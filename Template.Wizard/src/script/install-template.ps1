param (
    [string]$templateSource,
    [string]$projectName,
    [string]$framework,
    [string]$unitTest,
    [string]$proyectDb,
    [string]$sdk
)

if (-not $templateSource) {
    Write-Host "Debes proporcionar una fuente de template."
    exit 0
}
# Nombre del template y versión
$templateName = "Custom.Hexagonal.Template"
$templateVersion = "1.0.5"

# Verificar si el template ya está instalado
$installedTemplates = dotnet new --list | Out-String | findstr "Custom.Hexagonal.Template"

if ($installedTemplates) {
    Write-Output "El template '$templateName' ya está instalado. Desinstalando..."
    dotnet new uninstall $templateName | Out-Null
} else {
    Write-Output "El template '$templateName' no está instalado."
}

# Instalar desde NuGet.org
Write-Output "Instalando el template '$templateName::$templateVersion' desde NuGet..."
dotnet new install "$templateName::$templateVersion"

Write-Output "Creando proyecto '$projectName'..."

dotnet new CustomTemplate -o "$templateSource" -n "$projectName" --Framework "$framework" --IncludeSdk $sdk --IncludeDataTool $proyectDb --IncludeUnitTests $unitTest

# Se elimina proyecto de packaging, esta excluido en el template.config pero por algun motivo el proyecto
# queda asociado en la sln, la mejor forma que se encontro es eliminar la referencia del proyecto en la sln.
$removePackagingProject = "$projectName.Packaging\Custom.Hexagonal.$projectName\Custom.Hexagonal.$projectName.csproj"

Set-Location "$templateSource"

dotnet sln "$projectName.sln" remove $removePackagingProject

if ($sdk -ne $true) {
    $removeSdkUnitTestProject = "$projectName.Api.Sdk.Unit.Tests\$projectName.Api.Sdk.Unit.Tests.csproj"
    $removeSdkroject = "$projectName.Api.Sdk\$projectName.Api.Sdk.csproj"

    dotnet sln "$projectName.sln" remove $removeSdkUnitTestProject
    dotnet sln "$projectName.sln" remove $removeSdkroject
}

if ($proyectDb -ne $true) {
    $removeDataBaseProject = "$projectName.Api.Db\$projectName.Api.Db.sqlproj"
    dotnet sln "$projectName.sln" remove $removeDataBaseProject
}

if ($unitTest -ne $true) {
    $removeUnitTestsProject = "$projectName.Api.Unit.Tests\$projectName.Api.Unit.Tests.csproj"
    dotnet sln "$projectName.sln" remove $removeUnitTestsProject
}

exit 1
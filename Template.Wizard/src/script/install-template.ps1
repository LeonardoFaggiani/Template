param (
    [string]$templateSource,
    [string]$projectName,
    [string]$framework,
    [string]$unitTest,
    [string]$projectDb,
    [string]$sdk
)

# Nombre del template y versión
$templateName = "Custom.Api.Template"
$templateVersion = "1.0.0"
$totalSteps = 7
$currentStep = 0

function Emit-Progress {
    param ([int]$step, [int]$total)
    $percent = [math]::Round(($step / $total) * 100)
    Write-Output $percent
}

$currentStep++
Emit-Progress -step $currentStep -total $totalSteps

# Verificar si el template ya está instalado
$installedTemplates = dotnet new --list | Out-String | findstr "Custom.Api.Template"

if ($installedTemplates) {
    $output = dotnet new uninstall $templateName | Out-Null
}

# Instalar desde NuGet.org
$currentStep++
Emit-Progress -step $currentStep -total $totalSteps

$output = dotnet new install "$templateName::$templateVersion"

$currentStep++
Emit-Progress -step $currentStep -total $totalSteps

$output = dotnet new CustomApiTemplate -o "$templateSource" -n "$projectName" --Framework "$framework" --IncludeSdk $sdk --IncludeDataTool $projectDb --IncludeUnitTests $unitTest

# Se elimina proyecto de packaging, esta excluido en el template.config pero por algun motivo el proyecto
# queda asociado en la sln, la mejor forma que se encontro es eliminar la referencia del proyecto en la sln.
$currentStep++
Emit-Progress -step $currentStep -total $totalSteps

$removePackagingProject = "$projectName.Packaging\Custom.Api.$projectName\Custom.Api.$projectName.csproj"

Set-Location "$templateSource"

$output = dotnet sln "$projectName.sln" remove $removePackagingProject

if ($sdk -ne $true) {
    $currentStep++
    Emit-Progress -step $currentStep -total $totalSteps    

    $removeSdkUnitTestProject = "$projectName.Api.Sdk.Unit.Tests\$projectName.Api.Sdk.Unit.Tests.csproj"
    $removeSdkroject = "$projectName.Api.Sdk\$projectName.Api.Sdk.csproj"

    $output = dotnet sln "$projectName.sln" remove $removeSdkUnitTestProject
    $output = dotnet sln "$projectName.sln" remove $removeSdkroject
}

if ($projectDb -ne $true) {
    $currentStep++
    Emit-Progress -step $currentStep -total $totalSteps

    $removeDataBaseProject = "$projectName.Api.Db\$projectName.Api.Db.sqlproj"
    $output = dotnet sln "$projectName.sln" remove $removeDataBaseProject
}

if ($unitTest -ne $true) {
    $currentStep++
    Emit-Progress -step $currentStep -total $totalSteps

    $removeUnitTestsProject = "$projectName.Api.Unit.Tests\$projectName.Api.Unit.Tests.csproj"
    $output = dotnet sln "$projectName.sln" remove $removeUnitTestsProject
}

exit 1
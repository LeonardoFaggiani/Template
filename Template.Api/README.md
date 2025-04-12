# Project Purpose

The main idea behind this project was to provide a fast and low-cost solution for creating APIs with an architecture similar to what's commonly used in real-world jobs. The biggest pain point was that every time a team needed to start a new API from scratch, they would base it off an existing one — which often resulted in mistakes such as incorrect namespaces, missing implementations, or misconfigured classes.

# What Custom API Template solves

With the Custom API Template, you can consistently generate the same clean and standardized structure every time (fully customizable if needed), avoiding the recurring issues mentioned above.

It can be used via the dotnet CLI or through a UI to make the experience more user-friendly. 😉

# DotNet Command

Parameters:
    -o Location where the generated output will be placed.
    -n <name> Name of the output to be created. If no name is specified, the name of the output directory will be used.
    --Framework Generates the project using the selected .NET framework version.
    Possible values: net6.0, net7.0, and net8.0.
    ⚠️ Make sure the selected .NET framework is already installed on your machine.
    --IncludeSdk Includes an SDK project in the solution.
    --IncludeDataTool Includes a SQL Server Data Tools (SSDT) database project.
    --IncludeUnitTests Includes a Unit Test project.

# Command:

    dotnet new CustomApiTemplate -o "C://Example" -n "ProyectName" --Framework "net8.0" --IncludeSdk true --IncludeDataTool true --IncludeUnitTests true

    Mas información DotNet CLI:
    https://learn.microsoft.com/es-es/dotnet/core/tools/dotnet

# UI
![Alt Text](https://leofstorage.blob.core.windows.net/customapitemplate/CustomApiTemplateUI.jpg)
    
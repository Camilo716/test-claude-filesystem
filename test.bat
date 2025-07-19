@echo off
echo Running tests with coverage...

REM Run tests with coverage
dotnet test TodoApi.Tests/TodoApi.Tests.csproj ^
    --configuration Release ^
    --collect:"XPlat Code Coverage" ^
    --results-directory ./TestResults ^
    /p:CollectCoverage=true ^
    /p:CoverletOutputFormat=cobertura ^
    /p:CoverletOutput=./TestResults/coverage.xml

if %ERRORLEVEL% NEQ 0 (
    echo Tests failed!
    exit /b %ERRORLEVEL%
)

echo Tests completed successfully!
echo Coverage report generated in TestResults folder

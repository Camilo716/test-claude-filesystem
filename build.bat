@echo off
echo Building TodoApi...
dotnet build TodoApi.sln --configuration Release

if %ERRORLEVEL% NEQ 0 (
    echo Build failed!
    exit /b %ERRORLEVEL%
)

echo Build completed successfully!

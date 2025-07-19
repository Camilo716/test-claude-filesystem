#!/bin/bash
echo "Running tests with coverage..."

# Run tests with coverage
dotnet test TodoApi.Tests/TodoApi.Tests.csproj \
    --configuration Release \
    --collect:"XPlat Code Coverage" \
    --results-directory ./TestResults \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=cobertura \
    /p:CoverletOutput=./TestResults/coverage.xml

if [ $? -ne 0 ]; then
    echo "Tests failed!"
    exit 1
fi

echo "Tests completed successfully!"
echo "Coverage report generated in TestResults folder"

#!/bin/bash
echo "Building TodoApi..."
dotnet build TodoApi.sln --configuration Release

if [ $? -ne 0 ]; then
    echo "Build failed!"
    exit 1
fi

echo "Build completed successfully!"

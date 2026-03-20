#!/bin/bash
# Run both Boolk.API (backend) and Boolk.Client (frontend) concurrently

echo "Building solution..."
dotnet build Boolk.sln || exit 1

trap 'kill 0' EXIT

echo "Starting Boolk.API (backend)..."
dotnet run --project src/Boolk.API/Boolk.API.csproj --no-build &

echo "Starting Boolk.Client (frontend)..."
dotnet run --project src/Boolk.Client/Boolk.Client.csproj --no-build &

wait

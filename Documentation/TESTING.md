# Boolk Test Documentation

This document describes the testing strategy and execution for the Boolk solution.

## Test Structure

Tests are organized to mirror the `src` architecture, ensuring isolation and clarity.

| Layer | Type | Location | Description |
|-------|------|----------|-------------|
| **Domain** | Unit | `Boolk.Tests/Domain` | Tests pure business logic (entities, factories). Fast, no dependencies. |
| **Application** | Unit | `Boolk.Tests/Application` | Tests services and strategies using `Moq` for repositories. logic-focused. |
| **Infrastructure** | Integration | `Boolk.Tests/Infrastructure` | Tests actual repository implementations against **Firebase Emulator**. |
| **API** | Integration | `Boolk.Tests/API` | Tests Controllers via `WebApplicationFactory`. |
| **Client** | Component/Unit | `Boolk.Tests/Client` | Tests Blazor Components (`bUnit`) and API Clients (mocked `HttpClient`). |

## Prerequisites

### Unit Tests
- .NET 8 SDK

### Integration Tests
- **Firebase Emulator** is REQUIRED for `Infrastructure` and `API` tests.
- Install Firebase CLI: `npm install -g firebase-tools`
- Start Emulator:
  ```powershell
  firebase emulators:start --only firestore --project boolk-11546
  ```

## Running Tests

### Run All Tests
```powershell
dotnet test Boolk.Tests/Boolk.Tests.csproj
```

### Run Unit Tests Only (No Emulator needed)
You can filter tests by namespace or Trait (if added). Currently by folder:
```powershell
dotnet test --filter "FullyQualifiedName~Boolk.Tests.Domain|FullyQualifiedName~Boolk.Tests.Application|FullyQualifiedName~Boolk.Tests.Client"
```

### Coverage Report
To generate code coverage in Cobertura format:
```powershell
dotnet test --collect:"XPlat Code Coverage"
```
Results will be in `Boolk.Tests/TestResults/{guid}/coverage.cobertura.xml`.

## Recent Changes
- Updated `Boolk.Tests` to reference individual `src` projects instead of the solution wrapper.
- Added `WebApplicationFactory` support to `Boolk.API`.
- Structured tests into 5 distinct layers.

---
description: Autonomous testing agent - unit, integration, and E2E tests with xUnit
triggers: [manual]
autonomy: full
outputs: [code, reports]
---

# Testing Workflow

## Purpose

Fully autonomous test generation and execution using xUnit. Covers:
- Unit tests for Services, Strategies, Factories
- Integration tests with Firebase Emulator
- E2E tests for Blazor components with bUnit
- Code coverage reporting

## Prerequisites

- Boolk solution must compile
- For integration tests: Firebase CLI installed (`npm install -g firebase-tools`)

## Steps

### 1. Verify Test Infrastructure
// turbo
Check if `Boolk.Tests` project exists. If not, create it:

```powershell
cd d:\studiaProj\c#\projkon\Boolk
dotnet new xunit -n Boolk.Tests -o Boolk.Tests
dotnet sln add Boolk.Tests/Boolk.Tests.csproj
```

// turbo
Add required packages:
```powershell
cd Boolk.Tests
dotnet add package bunit --version 1.*
dotnet add package Moq --version 4.*
dotnet add package coverlet.collector --version 6.*
dotnet add package FluentAssertions --version 6.*
dotnet add reference ../Boolk.csproj
```

### 2. Create Test Directory Structure
// turbo
Ensure proper structure exists:

```
Boolk.Tests/
├── Unit/
│   ├── Services/
│   ├── Strategies/
│   ├── Factories/
│   └── Models/
├── Integration/
│   └── Repositories/
├── E2E/
│   └── Components/
├── Fixtures/
│   └── TestData/
└── Helpers/
```

### 3. Analyze Target Code
// turbo
When user requests tests for specific code:

1. Read the target file(s)
2. Identify all public methods and classes
3. Determine dependencies that need mocking
4. Plan test cases covering:
   - Happy path
   - Edge cases
   - Error conditions
   - Boundary values

### 4. Generate Unit Tests
// turbo
For each identified method, generate tests following pattern:

```csharp
public class {ClassName}Tests
{
    private readonly Mock<IDependency> _mockDependency;
    private readonly TargetClass _sut; // System Under Test

    public {ClassName}Tests()
    {
        _mockDependency = new Mock<IDependency>();
        _sut = new TargetClass(_mockDependency.Object);
    }

    [Fact]
    public void MethodName_WhenCondition_ShouldExpectedResult()
    {
        // Arrange
        // Act
        // Assert
    }

    [Theory]
    [InlineData(...)]
    public void MethodName_WithVariousInputs_ShouldBehaveCorrectly(...)
    {
        // Parameterized tests for multiple scenarios
    }
}
```

### 5. Generate Integration Tests (Firebase)
// turbo
For Repository tests using Firebase Emulator:

```csharp
public class FirebaseRepositoryTests : IAsyncLifetime
{
    // Setup emulator connection
    public async Task InitializeAsync()
    {
        // Connect to Firebase Emulator at localhost:8080
    }

    public async Task DisposeAsync()
    {
        // Cleanup test data
    }

    [Fact]
    public async Task Repository_WhenCRUDOperation_ShouldPersist()
    {
        // Test against emulator
    }
}
```

Provide instructions for starting emulator:
```powershell
firebase emulators:start --only firestore --project boolk-11546
```

### 6. Generate E2E/Component Tests (bUnit)
// turbo
For Blazor components:

```csharp
public class ComponentTests : TestContext
{
    [Fact]
    public void Component_WhenRendered_ShouldDisplayCorrectly()
    {
        // Arrange
        Services.AddScoped<IService>(sp => Mock.Of<IService>());
        
        // Act
        var cut = RenderComponent<TargetComponent>();
        
        // Assert
        cut.Find("h1").TextContent.Should().Be("Expected");
    }
}
```

### 7. Run Tests and Collect Coverage
// turbo
Execute all tests with coverage:

```powershell
dotnet test Boolk.Tests/Boolk.Tests.csproj --collect:"XPlat Code Coverage" --results-directory ./TestResults
```

### 8. Generate Test Report
// turbo
Create report at `.agent/reports/test-{timestamp}.md`:

```markdown
# Test Report - {date}

## Summary
- Tests Generated: {count}
- Tests Passed: {count}
- Tests Failed: {count}
- Code Coverage: {percentage}%

## Coverage by Component
| Component | Coverage |
|-----------|----------|
| Services | X% |
| Strategies | X% |
| Repositories | X% |

## New Tests Added
{list of new test files}

## Failed Tests (if any)
{details}
```

## Outputs

- `Boolk.Tests/**/*.cs` - Generated test files
- `.agent/reports/test-{timestamp}.md` - Test execution report
- `./TestResults/` - Coverage data

## Success Criteria

- All generated tests compile
- Test execution completes without infrastructure errors
- Coverage report generated
- No regressions in existing tests

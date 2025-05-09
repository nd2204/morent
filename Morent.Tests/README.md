# Morent.Tests

This project contains tests for the Morent car rental application using xUnit, Moq, and FluentAssertions.

## Test Organization

The tests are organized as follows:

- **Controllers**: Tests for the API controllers, focusing on the HTTP layer and ensuring proper responses
- **Unit**: Unit tests for individual components
  - **Features**: Tests for application features (queries, commands, handlers)
  - **Repositories**: Tests for repository implementations
- **Integration**: Integration tests that verify multiple components working together

## Test Categories

- **Unit Tests**: Verify the behavior of individual components in isolation using mocks for dependencies
- **Integration Tests**: Verify that components work together correctly with minimal mocking

## Technologies Used

- **xUnit**: Test framework
- **Moq**: Mocking library for creating test doubles
- **FluentAssertions**: For more readable assertions
- **EF Core InMemory**: For testing repositories without a real database

## Running Tests

### Using Visual Studio

1. Open the solution in Visual Studio
2. Right-click on the test project in Solution Explorer and select "Run Tests"

### Using Command Line

```bash
# Run all tests
dotnet test

# Run specific test category
dotnet test --filter "Category=Unit"
dotnet test --filter "Category=Integration"

# Run tests with specific trait
dotnet test --filter "FullyQualifiedName~Controllers"
```

## Test Naming Conventions

Tests follow the naming convention:

```
[Method]_[Scenario]_[ExpectedResult]
```

For example:
- `GetCarById_WhenCarExists_ReturnsOkWithCar`
- `GetCarById_WhenCarDoesNotExist_ReturnsNotFound`

## Current Issues and Recommended Solutions

There are currently some issues with the tests that need to be resolved:

1. **DbContext Mocking**: The tests fail with `Can not instantiate proxy of class: Morent.Infrastructure.Data.MorentDbContext` because EF Core's DbContext doesn't have a parameterless constructor. To fix this:
   - Create an interface `IMorentDbContext` with the necessary methods and properties
   - Have the `MorentDbContext` implement this interface
   - Use the interface in controllers and repositories instead of the concrete class
   - Mock the interface in tests instead of the concrete class

2. **Value Objects in InMemory Database**: Tests using the InMemory database provider fail with `KeyNotFoundException` for complex types and value objects. The InMemory provider doesn't fully support EF Core's owned entities and complex types. To fix this:
   - Use SQLite in-memory database instead of EF Core InMemory for repository tests
   - Configure the SQLite provider with `connection.CreateFunction()` for any custom functions

3. **Integration Tests**: The integration tests need the `Program` class to be public. Modify the `Program.cs` file to make the class public.

## Adding New Tests

When adding new tests:

1. Follow the existing folder structure
2. Use the established naming conventions
3. Ensure test coverage for:
   - Happy path (success scenarios)
   - Edge cases (error handling)
   - Validation logic
   - Authorization requirements (if applicable)

## Mocking Strategy

- Use Moq for mocking dependencies
- Mock at the interface level rather than concrete implementations
- Prefer using `It.IsAny<T>()` for parameters that don't affect behavior
- Use `It.Is<T>(...)` for conditions that matter to the test

## Best Practices

- Each test should verify a single behavior
- Use descriptive test names that explain the scenario
- Arrange-Act-Assert pattern for clarity
- Use `[Fact]` for fixed tests and `[Theory]` with `[InlineData]` for parameterized tests
- Keep test setup code minimal and focused on what's relevant to the test 
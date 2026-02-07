using Boolk.Domain.Entities;
using Boolk.Infrastructure.Persistence.Firebase;
using FluentAssertions;
using Google.Cloud.Firestore;
using Xunit;

namespace Boolk.Tests.Infrastructure;

public class FirebaseRestaurantRepositoryTests : IAsyncLifetime
{
    private readonly FirestoreDb _firestoreDb;
    private readonly FirebaseRestaurantRepository _repository;
    private readonly string _testProjectId = "boolk-11546";

    public FirebaseRestaurantRepositoryTests()
    {
        // Ensure FIRESTORE_EMULATOR_HOST is set. 
        // In a real CI/CD env, this would be set by the pipeline or docker compose.
        // For local runs, we assume it's set or we default to localhost:8080
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("FIRESTORE_EMULATOR_HOST")))
        {
            Environment.SetEnvironmentVariable("FIRESTORE_EMULATOR_HOST", "localhost:8080");
        }

        _firestoreDb = new FirestoreDbBuilder
        {
            ProjectId = _testProjectId,
            EmulatorDetection = Google.Api.Gax.EmulatorDetection.EmulatorOnly
        }.Build();

        _repository = new FirebaseRestaurantRepository(_firestoreDb);
    }

    public async Task InitializeAsync()
    {
        // Cleanup not strictly necessary if we use random IDs, but good practice.
    }

    public async Task DisposeAsync()
    {
        // Cleanup logic could go here
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistRestaurant()
    {
        // Arrange
        var restaurant = new ItalianRestaurant
        {
            Id = Guid.NewGuid(),
            Name = "Integration Test Pizza",
            City = "Test City"
        };

        // Act
        await _repository.CreateAsync(restaurant);

        // Assert
        var retrieved = await _repository.GetByIdAsync(restaurant.Id);
        retrieved.Should().NotBeNull();
        retrieved!.Name.Should().Be(restaurant.Name);
        retrieved.Should().BeOfType<ItalianRestaurant>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateFields()
    {
        // Arrange
        var restaurant = new Burgers
        {
            Id = Guid.NewGuid(),
            Name = "Original Burger",
            City = "City"
        };
        await _repository.CreateAsync(restaurant);

        // Act
        restaurant.Name = "Updated Burger";
        await _repository.UpdateAsync(restaurant);

        // Assert
        var retrieved = await _repository.GetByIdAsync(restaurant.Id);
        retrieved!.Name.Should().Be("Updated Burger");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRestaurant()
    {
        // Arrange
        var restaurant = new Kebab
        {
            Id = Guid.NewGuid(),
            Name = "Kebab to Delete",
            City = "City"
        };
        await _repository.CreateAsync(restaurant);

        // Act
        await _repository.DeleteAsync(restaurant.Id);

        // Assert
        var retrieved = await _repository.GetByIdAsync(restaurant.Id);
        retrieved.Should().BeNull();
    }
}

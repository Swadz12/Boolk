using System.Net;
using System.Net.Http.Json;
using Boolk.Domain.Entities;
using FluentAssertions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace Boolk.Tests.Integration.API;

/// <summary>
/// Integration tests for Menu API using WireMock.NET.
/// Tests HTTP layer behavior for future real API integration.
/// </summary>
public class MenuApiIntegrationTests : IDisposable
{
    private readonly WireMockServer _mockServer;
    private readonly HttpClient _httpClient;

    public MenuApiIntegrationTests()
    {
        _mockServer = WireMockServer.Start();
        _httpClient = new HttpClient 
        { 
            BaseAddress = new Uri(_mockServer.Url!) 
        };
    }

    [Fact]
    public async Task GetMenu_WithValidRestaurant_ReturnsMenuWithNutritionalData()
    {
        // Arrange
        _mockServer
            .Given(Request.Create()
                .WithPath("/api/menus")
                .WithParam("restaurant", "TestRestaurant")
                .WithParam("city", "Warsaw")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson(new
                {
                    id = Guid.NewGuid(),
                    restaurantId = Guid.NewGuid(),
                    name = "Main Menu",
                    lastUpdated = DateTime.UtcNow,
                    categories = new[]
                    {
                        new
                        {
                            id = Guid.NewGuid(),
                            name = "Main Courses",
                            displayOrder = 1,
                            items = new[]
                            {
                                new
                                {
                                    id = Guid.NewGuid(),
                                    name = "Grilled Salmon",
                                    description = "Fresh Atlantic salmon",
                                    price = 45.00,
                                    currency = "PLN",
                                    isAvailable = true,
                                    allergens = new[] { "fish" },
                                    dietaryTags = new[] { "keto" },
                                    nutrition = new
                                    {
                                        servingSize = "280g",
                                        servingWeightGrams = 280,
                                        calories = 420,
                                        protein = 38,
                                        carbohydrates = 0,
                                        fat = 22,
                                        saturatedFat = 4,
                                        sugar = 0,
                                        fiber = 0,
                                        sodium = 520
                                    }
                                }
                            }
                        }
                    }
                }));

        // Act
        var response = await _httpClient.GetAsync("/api/menus?restaurant=TestRestaurant&city=Warsaw");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Grilled Salmon");
        content.Should().Contain("420"); // calories
        content.Should().Contain("38");  // protein
    }

    [Fact]
    public async Task GetMenu_WithUnknownRestaurant_Returns404()
    {
        // Arrange
        _mockServer
            .Given(Request.Create()
                .WithPath("/api/menus")
                .WithParam("restaurant", "NonExistent")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(404)
                .WithBody("Restaurant menu not found"));

        // Act
        var response = await _httpClient.GetAsync("/api/menus?restaurant=NonExistent");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetMenu_WithServerError_Returns500()
    {
        // Arrange
        _mockServer
            .Given(Request.Create()
                .WithPath("/api/menus")
                .WithParam("restaurant", "ErrorCase")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(500)
                .WithBody("Internal server error"));

        // Act
        var response = await _httpClient.GetAsync("/api/menus?restaurant=ErrorCase");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task GetMenu_WithTimeout_HandlesGracefully()
    {
        // Arrange - simulate slow response
        _mockServer
            .Given(Request.Create()
                .WithPath("/api/menus")
                .WithParam("restaurant", "SlowRestaurant")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithDelay(TimeSpan.FromSeconds(2))
                .WithBody("{}"));

        // Act - with timeout
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        
        Func<Task> act = async () => await _httpClient.GetAsync(
            "/api/menus?restaurant=SlowRestaurant", 
            cts.Token);

        // Assert
        await act.Should().ThrowAsync<TaskCanceledException>();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        _mockServer.Stop();
        _mockServer.Dispose();
    }
}

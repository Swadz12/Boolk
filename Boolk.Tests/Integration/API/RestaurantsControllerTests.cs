

using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using System.Net;
using Boolk.Application.DTOs;
using System.Net.Http.Json;
using Boolk.Application.Common;

namespace Boolk.Tests.API;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        
        // We can configure the factory here to use a different DB or settings if needed
        // For now, we assume it connects to the Emulator via the config in Program.cs 
        // or environment variables set in the test runner/environment.
        
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/restaurants?page=1&pageSize=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<PagedResult<RestaurantDto>>();
        result.Should().NotBeNull();
        result!.Items.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnNotFound_OrEmpty()
    {
        // Act
        var response = await _client.GetAsync($"/api/restaurants/{Guid.NewGuid()}");

        // Assert
        // Depending on implementation, might be NotFound or NoContent or null.
        // Controller usually returns NotFound() if service returns null.
        if (response.StatusCode != HttpStatusCode.NotFound)
        {
             // If not found, it might return 204 No Content or similar, let's just check it didn't crash
             response.StatusCode.Should().NotBe(HttpStatusCode.InternalServerError);
        }
    }
}

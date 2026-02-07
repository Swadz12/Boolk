using Blazored.LocalStorage;
using Boolk.Application.Common;
using Boolk.Application.DTOs;

namespace Boolk.Client.ApiClients;

/// <summary>
/// HTTP client for restaurant API calls.
/// </summary>
public class RestaurantApiClient : ApiClientBase
{
    public RestaurantApiClient(HttpClient http, ILocalStorageService localStorage) 
        : base(http, localStorage) { }

    /// <summary>
    /// Get all restaurants with pagination.
    /// </summary>
    public async Task<PagedResult<RestaurantDto>?> GetAllAsync(int page = 1, int pageSize = 10)
    {
        return await GetAsync<PagedResult<RestaurantDto>>($"api/v1/restaurants?page={page}&pageSize={pageSize}");
    }

    /// <summary>
    /// Get a specific restaurant by ID.
    /// </summary>
    public async Task<RestaurantDto?> GetByIdAsync(Guid id)
    {
        return await GetAsync<RestaurantDto>($"api/v1/restaurants/{id}");
    }

    /// <summary>
    /// Create a new restaurant.
    /// </summary>
    public async Task<RestaurantDto?> CreateAsync(string name, string city, string type)
    {
        var request = new CreateRestaurantRequest(type, name, city);
        return await PostAsync<CreateRestaurantRequest, RestaurantDto>("api/v1/restaurants", request);
    }

    /// <summary>
    /// Update a restaurant.
    /// </summary>
    public async Task UpdateAsync(Guid id, string name, string city)
    {
        var request = new UpdateRestaurantRequest(name, city);
        await PutAsync($"api/v1/restaurants/{id}", request);
    }

    /// <summary>
    /// Delete a restaurant.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        await DeleteAsync($"api/v1/restaurants/{id}");
    }

    /// <summary>
    /// Get ranked restaurants by strategy.
    /// </summary>
    public async Task<IEnumerable<RestaurantDto>?> GetRankedAsync(string strategy = "best-value")
    {
        return await GetAsync<IEnumerable<RestaurantDto>>($"api/v1/restaurants/ranked?strategy={strategy}");
    }
}

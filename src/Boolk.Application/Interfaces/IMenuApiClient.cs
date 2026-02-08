using Boolk.Domain.Entities;

namespace Boolk.Application.Interfaces;

/// <summary>
/// External API client for fetching restaurant menu data.
/// </summary>
public interface IMenuApiClient
{
    /// <summary>
    /// Fetches menu data for a restaurant from external provider.
    /// </summary>
    /// <param name="restaurantName">Restaurant name for lookup</param>
    /// <param name="city">City for location-based matching</param>
    /// <param name="restaurantType">Type hint (e.g., "Asian", "Italian")</param>
    /// <returns>Menu data if found, null otherwise</returns>
    Task<Menu?> GetMenuAsync(string restaurantName, string city, string restaurantType);
}

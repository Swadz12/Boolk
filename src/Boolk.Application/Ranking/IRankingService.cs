using Boolk.Application.DTOs;

namespace Boolk.Application.Ranking;

/// <summary>
/// Interface for ranking service that applies strategies to rank restaurants.
/// </summary>
public interface IRankingService
{
    /// <summary>
    /// Get ranked restaurants using the specified strategy.
    /// </summary>
    Task<IEnumerable<RestaurantDto>> GetRankedRestaurantsAsync(string strategyName);
    
    /// <summary>
    /// Get available ranking strategy names.
    /// </summary>
    IEnumerable<string> GetAvailableStrategies();
}

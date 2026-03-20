using Boolk.Application.DTOs;

namespace Boolk.Application.Ranking;

public interface IRankingService
{
    Task<IEnumerable<RestaurantDto>> GetRankedRestaurantsAsync(string strategyName);
    
    IEnumerable<string> GetAvailableStrategies();
}

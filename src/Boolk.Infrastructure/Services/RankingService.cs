using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;
using Boolk.Application.Ranking;
using Boolk.Domain.Entities;

namespace Boolk.Infrastructure.Services;

public class RankingService : IRankingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Dictionary<string, IRankingStrategy> _strategies;

    public RankingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        
        var strategies = new List<IRankingStrategy>
        {
            new BestValueStrategy(),
            new CheapestStrategy(),
            new MostFillingStrategy()
        };
        
        _strategies = strategies.ToDictionary(s => s.Name, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<IEnumerable<RestaurantDto>> GetRankedRestaurantsAsync(string strategyName)
    {
        if (!_strategies.TryGetValue(strategyName, out var strategy))
        {
            strategy = _strategies.First().Value;
        }

        var restaurants = await _unitOfWork.Restaurants.GetAllAsync(0, 1000); 
        var reviews = await _unitOfWork.Reviews.GetAllAsync(); 

        
        var rankedRestaurants = strategy.Rank(restaurants.ToList(), reviews.ToList());
        
        return rankedRestaurants.Select(MapToDto);
    }

    public IEnumerable<string> GetAvailableStrategies()
    {
        return _strategies.Keys;
    }

    private static RestaurantDto MapToDto(RestaurantBase restaurant)
    {
        return new RestaurantDto(
            restaurant.Id,
            restaurant.Name,
            restaurant.City,
            restaurant.GetType().Name,
            restaurant.DisplayName,
            restaurant.DisplayIcon
        );
    }
}

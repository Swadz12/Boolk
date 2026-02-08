using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;
using Boolk.Application.Ranking;
using Boolk.Domain.Entities;

namespace Boolk.Infrastructure.Services;

/// <summary>
/// Implementation of ranking service using Strategy pattern.
/// </summary>
public class RankingService : IRankingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Dictionary<string, IRankingStrategy> _strategies;

    public RankingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        
        // Initialize strategies
        // In a more complex setup, these could be injected via DI
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
            // Fallback to default or empty if strategy not found
            // For now, let's default to BestValue if unknown, or throw
            strategy = _strategies.First().Value;
        }

        var restaurants = await _unitOfWork.Restaurants.GetAllAsync(0, 1000); // Get all for ranking
        var reviews = await _unitOfWork.Reviews.GetAllAsync(); // Simplified: fetch all reviews

        // Apply strategy
        // Note: reviews should ideally be fetched per restaurant or in bulk efficiently
        // For this implementation, we assume we have all necessary data
        
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

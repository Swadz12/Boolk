using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;
using Boolk.Application.Ranking;
using Boolk.Domain.Entities;

namespace Boolk.Infrastructure.Services;

/// <summary>
/// Ranking service implementation using Strategy pattern.
/// </summary>
public class RankingService : IRankingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Dictionary<string, IRankingStrategy> _strategies;

    public RankingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        
        // Register available strategies
        _strategies = new Dictionary<string, IRankingStrategy>(StringComparer.OrdinalIgnoreCase);
        RegisterStrategy(new BestValueStrategy());
        RegisterStrategy(new CheapestStrategy());
        RegisterStrategy(new MostFillingStrategy());
    }

    private void RegisterStrategy(IRankingStrategy strategy)
    {
        _strategies[strategy.Name] = strategy;
    }

    public async Task<IEnumerable<RestaurantDto>> GetRankedRestaurantsAsync(string strategyName)
    {
        // Get the strategy (default to best-value)
        var effectiveStrategy = strategyName ?? "best-value";
        
        if (!_strategies.TryGetValue(effectiveStrategy, out var strategy))
        {
            strategy = _strategies["best-value"];
        }

        // Get all restaurants and reviews
        var restaurants = (await _unitOfWork.Restaurants.GetAllAsync(0, 100)).ToList();
        var reviews = (await _unitOfWork.Reviews.GetAllAsync()).ToList();

        // Apply the ranking strategy
        var ranked = strategy.Rank(restaurants, reviews);

        // Map to DTOs
        return ranked.Select(r => new RestaurantDto(
            r.Id,
            r.Name,
            r.City,
            r.GetType().Name,
            r.DisplayName,
            r.DisplayIcon
        ));
    }

    public IEnumerable<string> GetAvailableStrategies()
    {
        return _strategies.Values.Select(s => s.Name);
    }
}

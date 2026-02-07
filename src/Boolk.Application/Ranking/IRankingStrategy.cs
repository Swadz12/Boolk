using Boolk.Domain.Entities;

namespace Boolk.Application.Ranking;

/// <summary>
/// Strategy interface for ranking restaurants.
/// Part of the Strategy pattern for flexible ranking algorithms.
/// </summary>
public interface IRankingStrategy
{
    /// <summary>
    /// Gets the unique name/key for this strategy.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Gets a human-readable description of the ranking logic.
    /// </summary>
    string Description { get; }
    
    /// <summary>
    /// Ranks restaurants based on their reviews using this strategy's algorithm.
    /// </summary>
    List<RestaurantBase> Rank(List<RestaurantBase> restaurants, List<Review> reviews);
}

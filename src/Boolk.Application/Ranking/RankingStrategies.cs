using Boolk.Domain.Entities;

namespace Boolk.Application.Ranking;

/// <summary>
/// Ranks restaurants by best value (satiety per price).
/// Higher satiety at lower price = higher rank.
/// </summary>
public class BestValueStrategy : IRankingStrategy
{
    public string Name => "best-value";
    public string Description => "Best value for money (highest satiety per price)";

    public List<RestaurantBase> Rank(List<RestaurantBase> restaurants, List<Review> reviews)
    {
        return restaurants
            .Select(restaurant =>
            {
                var restaurantReviews = reviews.Where(r => r.RestaurantId == restaurant.Id).ToList();
                if (!restaurantReviews.Any())
                    return (restaurant, score: 0.0);

                var avgPrice = restaurantReviews.Average(r => r.Price);
                var avgSatiety = restaurantReviews.Average(r => r.SatietyLevel);
                var score = avgPrice > 0 ? avgSatiety / avgPrice : 0;

                return (restaurant, score);
            })
            .OrderByDescending(x => x.score)
            .Select(x => x.restaurant)
            .ToList();
    }
}

/// <summary>
/// Ranks restaurants by average price (cheapest first).
/// </summary>
public class CheapestStrategy : IRankingStrategy
{
    public string Name => "cheapest";
    public string Description => "Lowest average price first";

    public List<RestaurantBase> Rank(List<RestaurantBase> restaurants, List<Review> reviews)
    {
        return restaurants
            .Select(restaurant =>
            {
                var restaurantReviews = reviews.Where(r => r.RestaurantId == restaurant.Id).ToList();
                if (!restaurantReviews.Any())
                    return (restaurant, avgPrice: double.MaxValue);

                var avgPrice = restaurantReviews.Average(r => r.Price);
                return (restaurant, avgPrice);
            })
            .OrderBy(x => x.avgPrice)
            .Select(x => x.restaurant)
            .ToList();
    }
}

/// <summary>
/// Ranks restaurants by satiety level (most filling first).
/// </summary>
public class MostFillingStrategy : IRankingStrategy
{
    public string Name => "most-filling";
    public string Description => "Highest average satiety level first";

    public List<RestaurantBase> Rank(List<RestaurantBase> restaurants, List<Review> reviews)
    {
        return restaurants
            .Select(restaurant =>
            {
                var restaurantReviews = reviews.Where(r => r.RestaurantId == restaurant.Id).ToList();
                if (!restaurantReviews.Any())
                    return (restaurant, avgSatiety: 0.0);

                var avgSatiety = restaurantReviews.Average(r => r.SatietyLevel);
                return (restaurant, avgSatiety);
            })
            .OrderByDescending(x => x.avgSatiety)
            .Select(x => x.restaurant)
            .ToList();
    }
}

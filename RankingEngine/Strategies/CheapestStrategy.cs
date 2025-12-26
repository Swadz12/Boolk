using Boolk.Models;
using Boolk.RankingEngine.Interfaces;

namespace Boolk.RankingEngine.Strategies;

public class CheapestStrategy : IRankingStrategy
{
    public List<RestaurantBase> CalculateScore(List<RestaurantBase> restaurants, List<Review> reviews)
    {
        var restaurantScores = restaurants.Select(restaurant =>
        {
            var restaurantReviews = reviews.Where(r => r.RestaurantId == restaurant.Id).ToList();
            if (!restaurantReviews.Any())
                return (restaurant, avgPrice: double.MaxValue);

            var avgPrice = restaurantReviews.Average(r => (double)r.Price);
            return (restaurant, avgPrice);
        })
        .OrderBy(x => x.avgPrice)
        .Select(x => x.restaurant)
        .ToList();

        return restaurantScores;
    }
}


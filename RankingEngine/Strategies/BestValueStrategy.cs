using Boolk.Models;
using Boolk.RankingEngine.Interfaces;

namespace Boolk.RankingEngine.Strategies;

public class BestValueStrategy : IRankingStrategy
{
    public List<RestaurantBase> CalculateScore(List<RestaurantBase> restaurants, List<Review> reviews)
    {
        var restaurantScores = restaurants.Select(restaurant =>
        {
            var restaurantReviews = reviews.Where(r => r.RestaurantId == restaurant.Id).ToList();
            if (!restaurantReviews.Any())
                return (restaurant, score: 0.0);

            var avgPrice = restaurantReviews.Average(r => (double)r.Price);
            var avgSatiety = restaurantReviews.Average(r => r.SatietyLevel);
            var score = avgSatiety / avgPrice; 

            return (restaurant, score);
        })
        .OrderByDescending(x => x.score)
        .Select(x => x.restaurant)
        .ToList();

        return restaurantScores;
    }
}


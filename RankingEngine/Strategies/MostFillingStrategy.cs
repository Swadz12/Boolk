using Boolk.Models;
using Boolk.RankingEngine.Interfaces;

namespace Boolk.RankingEngine.Strategies;

public class MostFillingStrategy : IRankingStrategy
{
    public List<RestaurantBase> CalculateScore(List<RestaurantBase> restaurants, List<Review> reviews)
    {
        var restaurantScores = restaurants.Select(restaurant =>
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

        return restaurantScores;
    }
}


using Boolk.Models;

namespace Boolk.RankingEngine.Interfaces;

public interface IRankingStrategy
{
    List<RestaurantBase> CalculateScore(List<RestaurantBase> restaurants, List<Review> reviews);
}


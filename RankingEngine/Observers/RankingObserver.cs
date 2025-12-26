using Boolk.Models;
using Boolk.RankingEngine.Interfaces;

namespace Boolk.RankingEngine.Observers;

public class RankingObserver : IObserver
{
    public void Update(RestaurantBase restaurant)
    {
        Console.WriteLine($"Ranking updated for restaurant: {restaurant.Name} in {restaurant.City}");
    }
}


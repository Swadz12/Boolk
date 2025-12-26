using Boolk.Models;

namespace Boolk.RankingEngine.Interfaces;

public interface IObserver
{
    void Update(RestaurantBase restaurant);
}


using Boolk.Models;
using Boolk.RankingEngine.Interfaces;

namespace Boolk.RankingEngine;

public class RankingService
{
    private readonly List<IObserver> _observers = new();
    private IRankingStrategy? _strategy;

    public RankingService()
    {
    }

    public void NotifyRankingsUpdated()
    {
        foreach (var observer in _observers.ToList()) 
        {
             observer.Update(null); 
        }
    }

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void SetStrategy(IRankingStrategy strategy)
    {
        _strategy = strategy;
    }

    public void Notify(RestaurantBase restaurant)
    {
        foreach (var observer in _observers)
        {
            observer.Update(restaurant);
        }
    }

    public List<RestaurantBase> GetTopRestaurants(List<RestaurantBase> restaurants, List<Review> reviews)
    {
        if (_strategy == null)
            throw new InvalidOperationException("Strategy not set. Call SetStrategy first.");

        var ranked = _strategy.CalculateScore(restaurants, reviews);
        
        return ranked;
    }
}


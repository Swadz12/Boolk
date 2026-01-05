using Boolk.Models;
using Boolk.RankingEngine.Interfaces;

namespace Boolk.RankingEngine.Observers;

public class RankingObserver : IObserver, IDisposable
{
    public event Action? OnRankingChanged;
    private readonly RankingService _rankingService;

    public RankingObserver(RankingService rankingService)
    {
        _rankingService = rankingService;
        
        _rankingService.Attach(this);
    }

    public void Update(RestaurantBase? restaurant)
    {
        OnRankingChanged?.Invoke();
    }

    public void Dispose()
    {
        _rankingService.Detach(this);
    }
}


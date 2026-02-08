using Boolk.Application.Events;
using Boolk.Application.Interfaces;
using Boolk.Application.Events;
using Boolk.Application.Interfaces;
using Boolk.Application.Ranking;
using MediatR;

namespace Boolk.Application.EventHandlers;

/// <summary>
/// Handles RankingChangedEvent by fetching updated rankings 
/// and pushing them to connected clients via IRealTimeNotifier.
/// </summary>
public class RankingChangedHandler : INotificationHandler<RankingChangedEvent>
{
    private readonly IRankingService _rankingService;
    private readonly IRealTimeNotifier _notifier;

    public RankingChangedHandler(
        IRankingService rankingService, 
        IRealTimeNotifier notifier)
    {
        _rankingService = rankingService;
        _notifier = notifier;
    }

    public async Task Handle(RankingChangedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[RankingChangedHandler] Received event: {notification.ChangeType} for RestaurantId: {notification.RestaurantId}");
        
        // Fetch updated rankings using default strategy
        var rankings = await _rankingService.GetRankedRestaurantsAsync("best-value");
        Console.WriteLine($"[RankingChangedHandler] Fetched {rankings.Count()} rankings, now pushing to clients...");
        
        // Push to all connected clients
        await _notifier.NotifyRankingsChangedAsync(rankings);
        Console.WriteLine("[RankingChangedHandler] Push completed");
    }
}

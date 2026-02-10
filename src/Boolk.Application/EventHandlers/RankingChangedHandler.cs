using Boolk.Application.Events;
using Boolk.Application.Interfaces;
using Boolk.Application.Ranking;
using MediatR;

namespace Boolk.Application.EventHandlers;

public class RankingChangedHandler : INotificationHandler<RankingChangedEvent>
{
    private readonly IRealTimeNotifier _notifier;

    public RankingChangedHandler(
        IRealTimeNotifier notifier)
    {
        _notifier = notifier;
    }

    public async Task Handle(RankingChangedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[RankingChangedHandler] Received event: {notification.ChangeType} for RestaurantId: {notification.RestaurantId}");
        
        Console.WriteLine("[RankingChangedHandler] Notification sent to invalidation handler");
        await _notifier.NotifyRankingsChangedAsync();
        Console.WriteLine("[RankingChangedHandler] Push completed");
    }
}

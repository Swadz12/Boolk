using Boolk.API.Hubs;
using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Boolk.API.Services;

/// <summary>
/// SignalR implementation of IRealTimeNotifier.
/// </summary>
public class SignalRNotifier : IRealTimeNotifier
{
    private readonly IHubContext<RankingHub, IRankingHubClient> _hubContext;

    public SignalRNotifier(IHubContext<RankingHub, IRankingHubClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyRankingsChangedAsync(IEnumerable<RestaurantDto> rankings)
    {
        Console.WriteLine($"[SignalRNotifier] Pushing {rankings.Count()} rankings to RankingWatchers group...");
        await _hubContext.Clients.Group("RankingWatchers")
            .ReceiveRankingsUpdate(rankings);
        Console.WriteLine("[SignalRNotifier] Push sent to SignalR hub");
    }

    public async Task NotifyRestaurantChangedAsync(RestaurantDto restaurant, string changeType)
    {
        await _hubContext.Clients.Group("RankingWatchers")
            .ReceiveRestaurantChange(restaurant, changeType);
    }
}

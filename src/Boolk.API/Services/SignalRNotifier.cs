using Boolk.API.Hubs;
using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Boolk.API.Services;

public class SignalRNotifier : IRealTimeNotifier
{
    private readonly IHubContext<RankingHub, IRankingHubClient> _hubContext;

    public SignalRNotifier(IHubContext<RankingHub, IRankingHubClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyRankingsChangedAsync()
    {
        Console.WriteLine("[SignalRNotifier] Sending invalidation signal to RankingWatchers group...");
        await _hubContext.Clients.Group("RankingWatchers")
            .ReceiveRankingsUpdate();
        Console.WriteLine("[SignalRNotifier] Signal sent");
    }

    public async Task NotifyRestaurantChangedAsync(RestaurantDto restaurant, string changeType)
    {
        await _hubContext.Clients.Group("RankingWatchers")
            .ReceiveRestaurantChange(restaurant, changeType);
    }
}

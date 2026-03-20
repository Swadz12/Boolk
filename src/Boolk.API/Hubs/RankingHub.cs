using Boolk.Application.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace Boolk.API.Hubs;

public class RankingHub : Hub<IRankingHubClient>
{
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"[RankingHub] Client connected: {Context.ConnectionId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, "RankingWatchers");
        Console.WriteLine($"[RankingHub] Client added to RankingWatchers group");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "RankingWatchers");
        await base.OnDisconnectedAsync(exception);
    }
}

public interface IRankingHubClient
{
    Task ReceiveRankingsUpdate();
    
    Task ReceiveRestaurantChange(RestaurantDto restaurant, string changeType);
}

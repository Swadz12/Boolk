using Boolk.Application.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace Boolk.API.Hubs;

/// <summary>
/// SignalR Hub for real-time ranking updates.
/// </summary>
public class RankingHub : Hub<IRankingHubClient>
{
    /// <summary>
    /// Called when a client connects. Adds them to the ranking watchers group.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"[RankingHub] Client connected: {Context.ConnectionId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, "RankingWatchers");
        Console.WriteLine($"[RankingHub] Client added to RankingWatchers group");
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Called when a client disconnects.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "RankingWatchers");
        await base.OnDisconnectedAsync(exception);
    }
}

/// <summary>
/// Strongly-typed client interface for RankingHub.
/// </summary>
public interface IRankingHubClient
{
    /// <summary>
    /// Receives updated rankings when they change.
    /// </summary>
    Task ReceiveRankingsUpdate(IEnumerable<RestaurantDto> rankings);
    
    /// <summary>
    /// Receives notification about a specific restaurant change.
    /// </summary>
    Task ReceiveRestaurantChange(RestaurantDto restaurant, string changeType);
}

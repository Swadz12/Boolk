using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;

namespace Boolk.Client.Services;

/// <summary>
/// No-op implementation of IRealTimeNotifier for Client-side execution.
/// In Hybrid mode, the Client app creates/updates data directly but cannot broadcast via SignalR Hub (which is on API).
/// </summary>
public class NoOpRealTimeNotifier : IRealTimeNotifier
{
    public Task NotifyRankingsChangedAsync(IEnumerable<RestaurantDto> rankings)
    {
        // Do nothing - Client cannot broadcast directly to other clients
        return Task.CompletedTask;
    }

    public Task NotifyRestaurantChangedAsync(RestaurantDto restaurant, string changeType)
    {
        // Do nothing
        return Task.CompletedTask;
    }
}

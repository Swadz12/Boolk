using Boolk.Application.DTOs;

namespace Boolk.Application.Interfaces;

/// <summary>
/// Abstraction for pushing real-time updates to connected clients.
/// Implementation will use SignalR in the API layer.
/// </summary>
public interface IRealTimeNotifier
{
    /// <summary>
    /// Notifies all connected clients that rankings have changed.
    /// </summary>
    Task NotifyRankingsChangedAsync(IEnumerable<RestaurantDto> rankings);
    
    /// <summary>
    /// Notifies clients about a specific restaurant update.
    /// </summary>
    Task NotifyRestaurantChangedAsync(RestaurantDto restaurant, string changeType);
}

using Boolk.Application.DTOs;

namespace Boolk.Application.Interfaces;

public interface IRealTimeNotifier
{
    Task NotifyRankingsChangedAsync();
    
    Task NotifyRestaurantChangedAsync(RestaurantDto restaurant, string changeType);
    

}

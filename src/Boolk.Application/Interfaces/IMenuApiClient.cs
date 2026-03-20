using Boolk.Domain.Entities;

namespace Boolk.Application.Interfaces;

public interface IMenuApiClient
{
    Task<Menu?> GetMenuAsync(string restaurantName, string city, string restaurantType);
}

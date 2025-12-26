using Boolk.Models;

namespace Boolk.Repositories.Interfaces;

public interface IRestaurantRepository
{
    Task<RestaurantBase?> GetByIdAsync(Guid id);
    Task<IEnumerable<RestaurantBase>> GetAllAsync();
    Task<RestaurantBase> CreateAsync(RestaurantBase restaurant);
    Task UpdateAsync(RestaurantBase restaurant);
    Task DeleteAsync(Guid id);
}


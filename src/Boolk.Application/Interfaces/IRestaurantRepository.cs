using Boolk.Domain.Entities;

namespace Boolk.Application.Interfaces;

/// <summary>
/// Repository interface for restaurant data access.
/// This abstraction allows swapping database implementations.
/// </summary>
public interface IRestaurantRepository
{
    Task<RestaurantBase?> GetByIdAsync(Guid id);
    Task<IEnumerable<RestaurantBase>> GetAllAsync(int skip, int take);
    Task<int> GetCountAsync();
    Task<RestaurantBase> CreateAsync(RestaurantBase restaurant);
    Task UpdateAsync(RestaurantBase restaurant);
    Task DeleteAsync(Guid id);
}

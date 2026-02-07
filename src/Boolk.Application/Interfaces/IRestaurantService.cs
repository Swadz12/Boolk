using Boolk.Application.Common;
using Boolk.Application.DTOs;

namespace Boolk.Application.Interfaces;

/// <summary>
/// Service interface for restaurant business logic.
/// </summary>
public interface IRestaurantService
{
    Task<PagedResult<RestaurantDto>> GetAllAsync(int page, int pageSize);
    Task<RestaurantDto?> GetByIdAsync(Guid id);
    Task<RestaurantDto> CreateAsync(CreateRestaurantRequest request);
    Task UpdateAsync(Guid id, UpdateRestaurantRequest request);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<RestaurantDto>> GetRankedAsync(string strategy);
}

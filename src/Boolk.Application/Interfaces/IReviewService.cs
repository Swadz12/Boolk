using Boolk.Application.DTOs;

namespace Boolk.Application.Interfaces;

/// <summary>
/// Service interface for review business logic.
/// </summary>
public interface IReviewService
{
    Task<IEnumerable<ReviewDto>> GetAllAsync();
    Task<IEnumerable<ReviewDto>> GetByRestaurantIdAsync(Guid restaurantId);
    Task<IEnumerable<ReviewDto>> GetByUserIdAsync(Guid userId);
    Task<ReviewDto?> GetByIdAsync(Guid id);
    Task<ReviewDto> CreateAsync(Guid userId, CreateReviewRequest request);
    Task DeleteAsync(Guid id);
}

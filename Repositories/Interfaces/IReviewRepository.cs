using Boolk.Models;

namespace Boolk.Repositories.Interfaces;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(Guid id);
    Task<IEnumerable<Review>> GetAllAsync();
    Task<IEnumerable<Review>> GetByRestaurantIdAsync(Guid restaurantId);
    Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId);
    Task<Review> CreateAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(Guid id);
}


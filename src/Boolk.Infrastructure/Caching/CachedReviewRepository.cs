using Boolk.Application.Interfaces;
using Boolk.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Boolk.Infrastructure.Caching;

/// <summary>
/// Caching decorator for IReviewRepository.
/// Caches read operations and invalidates on write operations.
/// </summary>
public class CachedReviewRepository : IReviewRepository
{
    private readonly IReviewRepository _inner;
    private readonly IMemoryCache _cache;
    private readonly CacheOptions _options;

    // Cache key patterns
    private const string AllReviewsKey = "reviews:all";
    private const string ReviewByIdKey = "reviews:id:{0}";
    private const string ReviewsByRestaurantKey = "reviews:restaurant:{0}";
    private const string ReviewsByUserKey = "reviews:user:{0}";

    public CachedReviewRepository(
        IReviewRepository inner,
        IMemoryCache cache,
        IOptions<CacheOptions> options)
    {
        _inner = inner;
        _cache = cache;
        _options = options.Value;
    }

    public async Task<Review?> GetByIdAsync(Guid id)
    {
        var key = string.Format(ReviewByIdKey, id);

        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _options.ReviewCacheDuration;
            return await _inner.GetByIdAsync(id);
        });
    }

    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        var result = await _cache.GetOrCreateAsync(AllReviewsKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _options.ReviewCacheDuration;
            return await _inner.GetAllAsync();
        });

        return result ?? Enumerable.Empty<Review>();
    }

    public async Task<IEnumerable<Review>> GetByRestaurantIdAsync(Guid restaurantId)
    {
        var key = string.Format(ReviewsByRestaurantKey, restaurantId);

        var result = await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _options.ReviewCacheDuration;
            return await _inner.GetByRestaurantIdAsync(restaurantId);
        });

        return result ?? Enumerable.Empty<Review>();
    }

    public async Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId)
    {
        var key = string.Format(ReviewsByUserKey, userId);

        var result = await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _options.ReviewCacheDuration;
            return await _inner.GetByUserIdAsync(userId);
        });

        return result ?? Enumerable.Empty<Review>();
    }

    // Write operations: Invalidate relevant caches
    public async Task<Review> CreateAsync(Review review)
    {
        InvalidateReviewCaches(review.RestaurantId, review.UserId);
        return await _inner.CreateAsync(review);
    }

    public async Task UpdateAsync(Review review)
    {
        InvalidateReviewCaches(review.RestaurantId, review.UserId);
        _cache.Remove(string.Format(ReviewByIdKey, review.Id));
        await _inner.UpdateAsync(review);
    }

    public async Task DeleteAsync(Guid id)
    {
        // For delete, we can't easily know restaurant/user IDs without fetching
        // So we invalidate the "all" cache and the specific ID cache
        _cache.Remove(AllReviewsKey);
        _cache.Remove(string.Format(ReviewByIdKey, id));
        await _inner.DeleteAsync(id);
    }

    private void InvalidateReviewCaches(Guid restaurantId, Guid userId)
    {
        _cache.Remove(AllReviewsKey);
        _cache.Remove(string.Format(ReviewsByRestaurantKey, restaurantId));
        _cache.Remove(string.Format(ReviewsByUserKey, userId));
    }
}

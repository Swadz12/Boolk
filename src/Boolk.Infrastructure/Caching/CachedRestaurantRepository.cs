using Boolk.Application.Interfaces;
using Boolk.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Boolk.Infrastructure.Caching;

public class CachedRestaurantRepository : IRestaurantRepository
{
    private readonly IRestaurantRepository _inner;
    private readonly IMemoryCache _cache;
    private readonly CacheOptions _options;

    private const string AllRestaurantsKey = "restaurants:all:{0}:{1}";
    private const string RestaurantByIdKey = "restaurants:id:{0}";
    private const string RestaurantCountKey = "restaurants:count";

    public CachedRestaurantRepository(
        IRestaurantRepository inner,
        IMemoryCache cache,
        IOptions<CacheOptions> options)
    {
        _inner = inner;
        _cache = cache;
        _options = options.Value;
    }

    public async Task<RestaurantBase?> GetByIdAsync(Guid id)
    {
        var key = string.Format(RestaurantByIdKey, id);

        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _options.RestaurantCacheDuration;
            return await _inner.GetByIdAsync(id);
        });
    }

    public async Task<IEnumerable<RestaurantBase>> GetAllAsync(int skip, int take)
    {
        var key = string.Format(AllRestaurantsKey, skip, take);

        var result = await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _options.RestaurantCacheDuration;
            return await _inner.GetAllAsync(skip, take);
        });

        return result ?? Enumerable.Empty<RestaurantBase>();
    }

    public async Task<int> GetCountAsync()
    {
        return await _cache.GetOrCreateAsync(RestaurantCountKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _options.RestaurantCacheDuration;
            return await _inner.GetCountAsync();
        });
    }

    public async Task<RestaurantBase> CreateAsync(RestaurantBase restaurant)
    {
        InvalidateRestaurantCaches();
        return await _inner.CreateAsync(restaurant);
    }

    public async Task UpdateAsync(RestaurantBase restaurant)
    {
        InvalidateRestaurantCaches();
        _cache.Remove(string.Format(RestaurantByIdKey, restaurant.Id));
        await _inner.UpdateAsync(restaurant);
    }

    public async Task DeleteAsync(Guid id)
    {
        InvalidateRestaurantCaches();
        _cache.Remove(string.Format(RestaurantByIdKey, id));
        await _inner.DeleteAsync(id);
    }

    private void InvalidateRestaurantCaches()
    {
        _cache.Remove(RestaurantCountKey);
        
    }
}

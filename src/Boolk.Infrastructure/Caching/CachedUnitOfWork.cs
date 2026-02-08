using Boolk.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Boolk.Infrastructure.Caching;

/// <summary>
/// Caching decorator for IUnitOfWork.
/// Wraps the inner UnitOfWork's repositories with caching decorators.
/// </summary>
public class CachedUnitOfWork : IUnitOfWork
{
    private readonly IUnitOfWork _inner;
    private readonly IMemoryCache _cache;
    private readonly IOptions<CacheOptions> _options;

    private IRestaurantRepository? _restaurants;
    private IReviewRepository? _reviews;

    public CachedUnitOfWork(
        IUnitOfWork inner,
        IMemoryCache cache,
        IOptions<CacheOptions> options)
    {
        _inner = inner;
        _cache = cache;
        _options = options;
    }

    public IRestaurantRepository Restaurants
        => _restaurants ??= new CachedRestaurantRepository(_inner.Restaurants, _cache, _options);

    public IReviewRepository Reviews
        => _reviews ??= new CachedReviewRepository(_inner.Reviews, _cache, _options);

    // Users are not cached (typically low-traffic, auth-sensitive)
    public IUserRepository Users => _inner.Users;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _inner.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        _inner.Dispose();
        GC.SuppressFinalize(this);
    }
}

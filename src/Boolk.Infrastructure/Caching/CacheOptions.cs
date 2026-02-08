namespace Boolk.Infrastructure.Caching;

/// <summary>
/// Configuration options for repository caching.
/// </summary>
public class CacheOptions
{
    /// <summary>
    /// Cache duration for restaurant data. Default: 10 minutes.
    /// </summary>
    public TimeSpan RestaurantCacheDuration { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Cache duration for review data. Default: 5 minutes.
    /// </summary>
    public TimeSpan ReviewCacheDuration { get; set; } = TimeSpan.FromMinutes(5);
}

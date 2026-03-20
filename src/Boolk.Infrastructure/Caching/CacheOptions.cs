namespace Boolk.Infrastructure.Caching;

public class CacheOptions
{
    public TimeSpan RestaurantCacheDuration { get; set; } = TimeSpan.FromMinutes(10);

    public TimeSpan ReviewCacheDuration { get; set; } = TimeSpan.FromMinutes(5);
}

using Boolk.Application.Interfaces;
using Boolk.Application.Ranking;
using Boolk.Domain.Factories;
using Boolk.Infrastructure.Auth;
using Boolk.Infrastructure.Caching;
using Boolk.Infrastructure.Persistence.Firebase;
using Boolk.Infrastructure.Services;
using Google.Cloud.Firestore;
using Microsoft.Extensions.DependencyInjection;

namespace Boolk.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        FirebaseConfig firebaseConfig,
        JwtSettings jwtSettings)
    {
        FirebaseInitializer.Initialize(firebaseConfig);
        
        services.AddSingleton(FirebaseInitializer.GetFirestoreDb());
        
        services.AddScoped<IUnitOfWork, FirebaseUnitOfWork>();
        
        services.AddSingleton(jwtSettings);
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        
        services.AddSingleton<RestaurantFactory>();
        
        services.AddScoped<IRankingService, RankingService>();
        
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRestaurantService, RestaurantService>();
        services.AddScoped<IReviewService, ReviewService>();
        
        services.AddScoped<IMenuApiClient, FakeMenuApiClient>();
        
        
        services.AddMemoryCache();
        
        services.Configure<CacheOptions>(options =>
        {
            options.RestaurantCacheDuration = TimeSpan.FromMinutes(10);
            options.ReviewCacheDuration = TimeSpan.FromMinutes(5);
        });
        
        services.Decorate<IUnitOfWork, CachedUnitOfWork>();
        
        return services;
    }
}

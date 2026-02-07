using Boolk.Application.Interfaces;
using Boolk.Application.Ranking;
using Boolk.Domain.Factories;
using Boolk.Infrastructure.Auth;
using Boolk.Infrastructure.Persistence.Firebase;
using Boolk.Infrastructure.Services;
using Google.Cloud.Firestore;
using Microsoft.Extensions.DependencyInjection;

namespace Boolk.Infrastructure;

/// <summary>
/// Extension methods for configuring Infrastructure services in DI container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Infrastructure layer services to the DI container.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        FirebaseConfig firebaseConfig,
        JwtSettings jwtSettings)
    {
        // Initialize Firebase
        FirebaseInitializer.Initialize(firebaseConfig);
        
        // Register FirestoreDb as singleton
        services.AddSingleton(FirebaseInitializer.GetFirestoreDb());
        
        // Register Unit of Work
        services.AddScoped<IUnitOfWork, FirebaseUnitOfWork>();
        
        // Register JWT settings and service
        services.AddSingleton(jwtSettings);
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        
        // Register Factory pattern
        services.AddSingleton<RestaurantFactory>();
        
        // Register Ranking Service (Strategy pattern)
        services.AddScoped<IRankingService, RankingService>();
        
        // Register application services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRestaurantService, RestaurantService>();
        services.AddScoped<IReviewService, ReviewService>();
        
        return services;
    }
}


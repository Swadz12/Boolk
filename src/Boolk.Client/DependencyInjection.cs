using Blazored.LocalStorage;
using Boolk.Client.ApiClients;
using Boolk.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Boolk.Client;

/// <summary>
/// Extension methods for configuring Client services in DI container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Client layer services (API clients, authentication) to the DI container.
    /// </summary>
    public static IServiceCollection AddBoolkClient(
        this IServiceCollection services, 
        string apiBaseUrl = "https://localhost:5001")
    {
        // Add LocalStorage for token persistence
        services.AddBlazoredLocalStorage();
        
        // Configure HttpClient with base address
        services.AddScoped(sp => new HttpClient 
        { 
            BaseAddress = new Uri(apiBaseUrl) 
        });
        
        // Register API clients
        services.AddScoped<AuthApiClient>();
        services.AddScoped<RestaurantApiClient>();
        services.AddScoped<ReviewApiClient>();
        
        // Register authentication state provider
        services.AddScoped<JwtAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp => 
            sp.GetRequiredService<JwtAuthenticationStateProvider>());
        
        // Add authorization services
        services.AddAuthorizationCore();
        
        return services;
    }
}

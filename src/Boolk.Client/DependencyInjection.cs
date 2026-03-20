using Blazored.LocalStorage;
using Boolk.Client.ApiClients;
using Boolk.Client.Services;
using Boolk.Client.Auth;
using Boolk.Client.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Boolk.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddBoolkClient(
        this IServiceCollection services, 
        string apiBaseUrl = "https://localhost:5001")
    {
        services.AddBlazoredLocalStorage();
        
        services.AddScoped(sp => 
        {
            var handler = new HttpClientHandler();
            
            if (apiBaseUrl.Contains("localhost"))
            {
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            }

            return new HttpClient(handler) 
            { 
                BaseAddress = new Uri(apiBaseUrl) 
            };
        });
        
        services.AddScoped<AuthApiClient>();
        services.AddScoped<RestaurantApiClient>();
        services.AddScoped<ReviewApiClient>();
        
        services.AddScoped<RankingRealTimeService>(sp => 
            new RankingRealTimeService($"{apiBaseUrl}/hubs/ranking"));
        
        services.AddScoped<JwtAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp => 
            sp.GetRequiredService<JwtAuthenticationStateProvider>());
        
        services.AddAuthorizationCore();

        // ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegisterViewModel>();
        services.AddTransient<IndexViewModel>();
        services.AddTransient<RestaurantDetailViewModel>();
        services.AddTransient<ProfileViewModel>();
        services.AddTransient<RestaurantsViewModel>();
        services.AddTransient<ReviewsViewModel>();
        services.AddTransient<DashboardViewModel>();

        return services;
    }
}

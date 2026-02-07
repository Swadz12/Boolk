using Blazored.LocalStorage;
using Boolk.Application.DTOs;

namespace Boolk.Client.ApiClients;

/// <summary>
/// HTTP client for authentication API calls.
/// </summary>
public class AuthApiClient : ApiClientBase
{
    public AuthApiClient(HttpClient http, ILocalStorageService localStorage) 
        : base(http, localStorage) { }

    /// <summary>
    /// Login and store the JWT token.
    /// </summary>
    public async Task<AuthResponse?> LoginAsync(string email, string password)
    {
        var request = new LoginRequest(email, password);
        var response = await PostPublicAsync<LoginRequest, AuthResponse>("api/v1/auth/login", request);
        
        if (response?.Success == true && !string.IsNullOrEmpty(response.Token))
        {
            await LocalStorage.SetItemAsStringAsync("authToken", response.Token);
        }
        
        return response;
    }

    /// <summary>
    /// Register a new user and store the JWT token.
    /// </summary>
    public async Task<AuthResponse?> RegisterAsync(string email, string name, string password, DateTime birthDate)
    {
        var request = new RegisterRequest(email, name, birthDate, password);
        var response = await PostPublicAsync<RegisterRequest, AuthResponse>("api/v1/auth/register", request);
        
        if (response?.Success == true && !string.IsNullOrEmpty(response.Token))
        {
            await LocalStorage.SetItemAsStringAsync("authToken", response.Token);
        }
        
        return response;
    }

    /// <summary>
    /// Get current logged in user info.
    /// </summary>
    public async Task<UserDto?> GetCurrentUserAsync()
    {
        try
        {
            return await GetAsync<UserDto>("api/v1/auth/me");
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Logout - clear the stored token.
    /// </summary>
    public async Task LogoutAsync()
    {
        await LocalStorage.RemoveItemAsync("authToken");
        Http.DefaultRequestHeaders.Authorization = null;
    }

    /// <summary>
    /// Check if user is authenticated (has token).
    /// </summary>
    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await LocalStorage.GetItemAsStringAsync("authToken");
        return !string.IsNullOrEmpty(token);
    }
}

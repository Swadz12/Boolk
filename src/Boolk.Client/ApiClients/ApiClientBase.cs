using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;

namespace Boolk.Client.ApiClients;

/// <summary>
/// Base HTTP client for API communication with JWT token handling.
/// </summary>
public class ApiClientBase
{
    protected readonly HttpClient Http;
    protected readonly ILocalStorageService LocalStorage;
    protected static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClientBase(HttpClient http, ILocalStorageService localStorage)
    {
        Http = http;
        LocalStorage = localStorage;
    }

    /// <summary>
    /// Adds JWT authorization header if token exists.
    /// </summary>
    protected async Task AddAuthHeaderAsync()
    {
        var token = await LocalStorage.GetItemAsStringAsync("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            // Remove quotes if stored as JSON string
            token = token.Trim('"');
            Http.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    /// <summary>
    /// Makes an authenticated GET request.
    /// </summary>
    protected async Task<T?> GetAsync<T>(string url)
    {
        await AddAuthHeaderAsync();
        return await Http.GetFromJsonAsync<T>(url, JsonOptions);
    }

    /// <summary>
    /// Makes an authenticated POST request.
    /// </summary>
    protected async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        await AddAuthHeaderAsync();
        var response = await Http.PostAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions);
    }

    /// <summary>
    /// Makes an unauthenticated POST request (for login/register).
    /// </summary>
    protected async Task<TResponse?> PostPublicAsync<TRequest, TResponse>(string url, TRequest data)
    {
        var response = await Http.PostAsJsonAsync(url, data);
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions);
    }

    /// <summary>
    /// Makes an authenticated PUT request.
    /// </summary>
    protected async Task PutAsync<TRequest>(string url, TRequest data)
    {
        await AddAuthHeaderAsync();
        var response = await Http.PutAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Makes an authenticated DELETE request.
    /// </summary>
    protected async Task DeleteAsync(string url)
    {
        await AddAuthHeaderAsync();
        var response = await Http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }
}

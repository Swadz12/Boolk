using Boolk.Application.DTOs;
using Microsoft.AspNetCore.SignalR.Client;

namespace Boolk.Client.Services;

/// <summary>
/// Manages SignalR connection for real-time ranking updates.
/// </summary>
public class RankingRealTimeService : IAsyncDisposable
{
    private HubConnection? _connection;
    private readonly string _hubUrl;
    
    /// <summary>
    /// Fired when rankings are updated from the server.
    /// </summary>
    public event Action<IEnumerable<RestaurantDto>>? OnRankingsUpdated;
    
    /// <summary>
    /// Fired when a specific restaurant changes.
    /// </summary>
    public event Action<RestaurantDto, string>? OnRestaurantChanged;
    
    /// <summary>
    /// Current connection state.
    /// </summary>
    public HubConnectionState State => _connection?.State ?? HubConnectionState.Disconnected;

    public RankingRealTimeService(string hubUrl)
    {
        _hubUrl = hubUrl;
    }

    /// <summary>
    /// Starts the SignalR connection with automatic reconnection.
    /// </summary>
    public async Task StartAsync()
    {
        // If already connected, don't reconnect
        if (_connection != null && _connection.State == HubConnectionState.Connected)
            return;

        // If connection exists but is not connected, dispose and recreate
        if (_connection != null)
        {
            await _connection.DisposeAsync();
            _connection = null;
        }

        _connection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .WithAutomaticReconnect(new[] { 
                TimeSpan.Zero, 
                TimeSpan.FromSeconds(2), 
                TimeSpan.FromSeconds(10), 
                TimeSpan.FromSeconds(30) 
            })
            .Build();

        // Register handlers
        _connection.On<IEnumerable<RestaurantDto>>("ReceiveRankingsUpdate", rankings =>
        {
            Console.WriteLine($"[RankingRealTimeService] Received {rankings.Count()} rankings update");
            OnRankingsUpdated?.Invoke(rankings);
        });

        _connection.On<RestaurantDto, string>("ReceiveRestaurantChange", (restaurant, changeType) =>
        {
            OnRestaurantChanged?.Invoke(restaurant, changeType);
        });

        await _connection.StartAsync();
        Console.WriteLine($"[RankingRealTimeService] Connected to SignalR hub");
    }

    /// <summary>
    /// Stops the SignalR connection.
    /// </summary>
    public async Task StopAsync()
    {
        if (_connection != null)
        {
            await _connection.StopAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }
    }
}

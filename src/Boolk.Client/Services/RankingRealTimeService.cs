using Boolk.Application.DTOs;
using Microsoft.AspNetCore.SignalR.Client;

namespace Boolk.Client.Services;

public class RankingRealTimeService : IAsyncDisposable
{
    private HubConnection? _connection;
    private readonly string _hubUrl;
    
    public event Action<IEnumerable<RestaurantDto>>? OnRankingsUpdated;
    
    public event Action<RestaurantDto, string>? OnRestaurantChanged;
    
    public HubConnectionState State => _connection?.State ?? HubConnectionState.Disconnected;

    public RankingRealTimeService(string hubUrl)
    {
        _hubUrl = hubUrl;
    }

    public async Task StartAsync()
    {
        if (_connection != null && _connection.State == HubConnectionState.Connected)
            return;

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

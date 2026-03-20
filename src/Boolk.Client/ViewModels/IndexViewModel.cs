using Boolk.Application.DTOs;
using Boolk.Client.ApiClients;

namespace Boolk.Client.ViewModels;

public class IndexViewModel : ViewModelBase
{
    private readonly RestaurantApiClient _restaurantClient;

    public IndexViewModel(RestaurantApiClient restaurantClient)
    {
        _restaurantClient = restaurantClient;
    }

    private List<RestaurantDto>? _rankedRestaurants;
    public List<RestaurantDto>? RankedRestaurants
    {
        get => _rankedRestaurants;
        private set => SetProperty(ref _rankedRestaurants, value);
    }

    private string _selectedStrategy = "best-value";
    public string SelectedStrategy
    {
        get => _selectedStrategy;
        set => SetProperty(ref _selectedStrategy, value);
    }

    private bool _isLoading;
    public bool IsLoading { get => _isLoading; private set => SetProperty(ref _isLoading, value); }

    public async Task LoadRankedRestaurantsAsync()
    {
        IsLoading = true;
        NotifyStateChanged();
        try
        {
            await Task.Delay(300);
            RankedRestaurants = (await _restaurantClient.GetRankedAsync(SelectedStrategy))?.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading restaurants: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public string GetRankClass(int rank)
    {
        return rank switch
        {
            1 => "gold",
            2 => "silver",
            3 => "bronze",
            _ => ""
        };
    }
}

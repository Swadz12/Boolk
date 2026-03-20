using Boolk.Application.DTOs;
using Boolk.Client.ApiClients;

namespace Boolk.Client.ViewModels;

public class RestaurantDetailViewModel : ViewModelBase
{
    private readonly RestaurantApiClient _restaurantClient;
    private readonly ReviewApiClient _reviewClient;

    public RestaurantDetailViewModel(RestaurantApiClient restaurantClient, ReviewApiClient reviewClient)
    {
        _restaurantClient = restaurantClient;
        _reviewClient = reviewClient;
    }

    private RestaurantDto? _restaurant;
    public RestaurantDto? Restaurant { get => _restaurant; private set => SetProperty(ref _restaurant, value); }

    private List<ReviewDto>? _reviews;
    public List<ReviewDto>? Reviews { get => _reviews; private set => SetProperty(ref _reviews, value); }

    private bool _isLoading = true;
    public bool IsLoading { get => _isLoading; private set => SetProperty(ref _isLoading, value); }

    private int _reviewCount;
    public int ReviewCount { get => _reviewCount; private set => SetProperty(ref _reviewCount, value); }

    private double _averagePrice;
    public double AveragePrice { get => _averagePrice; private set => SetProperty(ref _averagePrice, value); }

    private decimal _averageSatiety;
    public decimal AverageSatiety { get => _averageSatiety; private set => SetProperty(ref _averageSatiety, value); }

    public async Task LoadAsync(Guid restaurantId)
    {
        IsLoading = true;
        NotifyStateChanged();
        try
        {
            await Task.Delay(300);
            Restaurant = await _restaurantClient.GetByIdAsync(restaurantId);

            if (Restaurant != null)
            {
                Reviews = (await _reviewClient.GetByRestaurantAsync(restaurantId))?.ToList();
                ReviewCount = Reviews?.Count ?? 0;

                if (Reviews != null && Reviews.Any())
                {
                    AveragePrice = Reviews.Average(r => r.Price);
                    AverageSatiety = Reviews.Average(r => (decimal)r.SatietyLevel);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading restaurant: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
}

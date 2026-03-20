using Boolk.Application.DTOs;
using Boolk.Client.ApiClients;
using Boolk.Client.Services;

namespace Boolk.Client.ViewModels;

public class RestaurantsViewModel : ViewModelBase, IAsyncDisposable
{
    private readonly RestaurantApiClient _restaurantClient;
    private readonly RankingRealTimeService _realTimeService;

    public RestaurantsViewModel(RestaurantApiClient restaurantClient, RankingRealTimeService realTimeService)
    {
        _restaurantClient = restaurantClient;
        _realTimeService = realTimeService;
    }

    public NewRestaurantModel NewRestaurant { get; set; } = new();

    private string _restaurantType = "fastfood";
    public string RestaurantType { get => _restaurantType; set => SetProperty(ref _restaurantType, value); }

    private List<RestaurantDto>? _restaurants;
    public List<RestaurantDto>? Restaurants { get => _restaurants; private set => SetProperty(ref _restaurants, value); }

    private int _currentPage = 1;
    public int CurrentPage { get => _currentPage; private set => SetProperty(ref _currentPage, value); }

    private int _pageSize = 6;
    public int PageSize { get => _pageSize; set => SetProperty(ref _pageSize, value); }

    private int _totalRestaurants;
    public int TotalRestaurants { get => _totalRestaurants; private set => SetProperty(ref _totalRestaurants, value); }

    private int _totalPages = 1;
    public int TotalPages { get => _totalPages; private set => SetProperty(ref _totalPages, value); }

    private bool _hasMorePages;
    public bool HasMorePages { get => _hasMorePages; private set => SetProperty(ref _hasMorePages, value); }

    private string _message = "";
    public string Message { get => _message; set => SetProperty(ref _message, value); }

    private bool _isSubmitting;
    public bool IsSubmitting { get => _isSubmitting; private set => SetProperty(ref _isSubmitting, value); }

    public Func<Func<Task>, Task>? InvokeAsync { get; set; }

    public async Task InitializeAsync()
    {
        _realTimeService.OnRankingsUpdated += HandleRankingsUpdate;
        await LoadRestaurantsAsync();
    }

    public async Task StartSignalRAsync()
    {
        try
        {
            await _realTimeService.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Restaurants] SignalR connection failed: {ex.Message}");
        }
    }

    private void HandleRankingsUpdate(IEnumerable<RestaurantDto> rankings)
    {
        if (InvokeAsync != null)
        {
            _ = InvokeAsync(async () =>
            {
                await LoadRestaurantsAsync();
                NotifyStateChanged();
            });
        }
    }

    public async Task LoadRestaurantsAsync()
    {
        try
        {
            var result = await _restaurantClient.GetAllAsync(CurrentPage, PageSize);
            if (result != null)
            {
                Restaurants = result.Items.ToList();
                TotalRestaurants = result.TotalCount;
                TotalPages = (int)Math.Ceiling((double)result.TotalCount / PageSize);
                HasMorePages = CurrentPage < TotalPages;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading restaurants: {ex.Message}");
        }
    }

    public async Task CreateRestaurantAsync()
    {
        IsSubmitting = true;
        Message = "";
        NotifyStateChanged();
        try
        {
            await _restaurantClient.CreateAsync(NewRestaurant.Name, NewRestaurant.City, RestaurantType);
            Message = $"Restaurant '{NewRestaurant.Name}' added successfully!";
            NewRestaurant = new NewRestaurantModel();
            RestaurantType = "fastfood";

            _currentPage = 1;
            await LoadRestaurantsAsync();

            await Task.Delay(3000);
            Message = "";
        }
        catch (Exception ex)
        {
            Message = $"Error: {ex.Message}";
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    public async Task NextPageAsync()
    {
        if (HasMorePages)
        {
            _currentPage++;
            await LoadRestaurantsAsync();
        }
    }

    public async Task PrevPageAsync()
    {
        if (CurrentPage > 1)
        {
            _currentPage--;
            await LoadRestaurantsAsync();
        }
    }

    public async Task RefreshAsync()
    {
        await LoadRestaurantsAsync();
    }

    public async ValueTask DisposeAsync()
    {
        _realTimeService.OnRankingsUpdated -= HandleRankingsUpdate;
        await _realTimeService.DisposeAsync();
    }

    public class NewRestaurantModel
    {
        public string Name { get; set; } = "";
        public string City { get; set; } = "";
    }
}

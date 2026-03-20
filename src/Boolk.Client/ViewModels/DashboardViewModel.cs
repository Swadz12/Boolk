using Boolk.Application.DTOs;
using Boolk.Client.ApiClients;
using Boolk.Client.Services;

namespace Boolk.Client.ViewModels;

public class DashboardViewModel : ViewModelBase, IAsyncDisposable
{
    private readonly RestaurantApiClient _restaurantClient;
    private readonly ReviewApiClient _reviewClient;
    private readonly RankingRealTimeService _realTimeService;

    public DashboardViewModel(
        RestaurantApiClient restaurantClient,
        ReviewApiClient reviewClient,
        RankingRealTimeService realTimeService)
    {
        _restaurantClient = restaurantClient;
        _reviewClient = reviewClient;
        _realTimeService = realTimeService;
    }

    private int _totalRestaurants;
    public int TotalRestaurants { get => _totalRestaurants; private set => SetProperty(ref _totalRestaurants, value); }

    private int _totalReviews;
    public int TotalReviews { get => _totalReviews; private set => SetProperty(ref _totalReviews, value); }

    private double _averagePrice;
    public double AveragePrice { get => _averagePrice; private set => SetProperty(ref _averagePrice, value); }

    private decimal _averageSatiety;
    public decimal AverageSatiety { get => _averageSatiety; private set => SetProperty(ref _averageSatiety, value); }

    private List<RestaurantTypeStat> _restaurantTypeStats = new();
    public List<RestaurantTypeStat> RestaurantTypeStats
    {
        get => _restaurantTypeStats;
        private set => SetProperty(ref _restaurantTypeStats, value);
    }

    private List<RestaurantDto>? _topReviewedRestaurants;
    public List<RestaurantDto>? TopReviewedRestaurants
    {
        get => _topReviewedRestaurants;
        private set => SetProperty(ref _topReviewedRestaurants, value);
    }

    private List<RestaurantDto>? _quickRankings;
    public List<RestaurantDto>? QuickRankings
    {
        get => _quickRankings;
        private set => SetProperty(ref _quickRankings, value);
    }

    private string _activeStrategy = "best-value";
    public string ActiveStrategy { get => _activeStrategy; private set => SetProperty(ref _activeStrategy, value); }

    private bool _isLoading;
    public bool IsLoading { get => _isLoading; private set => SetProperty(ref _isLoading, value); }

    private List<ReviewDto>? _allReviews;
    private List<RestaurantDto>? _allRestaurants;

    public Func<Func<Task>, Task>? InvokeAsync { get; set; }

    public async Task InitializeAsync()
    {
        _realTimeService.OnRankingsUpdated += HandleRankingsUpdate;
        await LoadDashboardDataAsync();
        await LoadQuickRankingAsync(ActiveStrategy);
    }

    public async Task StartSignalRAsync()
    {
        try
        {
            await _realTimeService.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Dashboard] SignalR connection failed: {ex.Message}");
        }
    }

    private void HandleRankingsUpdate(IEnumerable<RestaurantDto> rankings)
    {
        if (InvokeAsync != null)
        {
            _ = InvokeAsync(async () =>
            {
                await LoadDashboardDataAsync();
                await LoadQuickRankingAsync(ActiveStrategy);
                NotifyStateChanged();
            });
        }
    }

    public async Task LoadDashboardDataAsync()
    {
        try
        {
            var restaurantResult = await _restaurantClient.GetAllAsync(1, 1000);
            if (restaurantResult != null)
            {
                _allRestaurants = restaurantResult.Items.ToList();
                TotalRestaurants = restaurantResult.TotalCount;
            }

            _allReviews = (await _reviewClient.GetAllAsync())?.ToList() ?? new();
            TotalReviews = _allReviews.Count;

            if (_allReviews.Any())
            {
                AveragePrice = _allReviews.Average(r => r.Price);
                AverageSatiety = _allReviews.Average(r => (decimal)r.SatietyLevel);
            }

            RestaurantTypeStats = new List<RestaurantTypeStat>
            {
                new() { Label = "Fast Food", Count = _allRestaurants?.Count(r => r.Type == "FastFoodRestaurant") ?? 0, Gradient = "var(--primary-gradient)", BarHeight = 8 },
                new() { Label = "Student Bar", Count = _allRestaurants?.Count(r => r.Type == "StudentBar") ?? 0, Gradient = "var(--secondary-gradient)", BarHeight = 8 },
                new() { Label = "Premium", Count = _allRestaurants?.Count(r => r.Type == "PremiumRestaurant") ?? 0, Gradient = "var(--success-gradient)", BarHeight = 8 },
                new() { Label = "Asian", Count = _allRestaurants?.Count(r => r.Type == "AsianRestaurant") ?? 0, Gradient = "var(--primary-gradient)", BarHeight = 8 },
                new() { Label = "Burgers", Count = _allRestaurants?.Count(r => r.Type == "Burgers") ?? 0, Gradient = "var(--secondary-gradient)", BarHeight = 8 },
                new() { Label = "Sushi", Count = _allRestaurants?.Count(r => r.Type == "Sushi") ?? 0, Gradient = "var(--success-gradient)", BarHeight = 8 },
                new() { Label = "Italian", Count = _allRestaurants?.Count(r => r.Type == "ItalianRestaurant") ?? 0, Gradient = "var(--primary-gradient)", BarHeight = 8 },
                new() { Label = "Kebab", Count = _allRestaurants?.Count(r => r.Type == "Kebab") ?? 0, Gradient = "var(--secondary-gradient)", BarHeight = 8 }
            };

            if (_allReviews.Any() && _allRestaurants != null)
            {
                var reviewCounts = _allReviews
                    .GroupBy(r => r.RestaurantId)
                    .Select(g => new { RestaurantId = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToList();

                TopReviewedRestaurants = reviewCounts
                    .Select(rc => _allRestaurants.FirstOrDefault(r => r.Id == rc.RestaurantId))
                    .Where(r => r != null)
                    .Cast<RestaurantDto>()
                    .ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading dashboard data: {ex.Message}");
        }
    }

    public async Task LoadQuickRankingAsync(string strategy)
    {
        IsLoading = true;
        ActiveStrategy = strategy;
        NotifyStateChanged();
        try
        {
            await Task.Delay(300);
            QuickRankings = (await _restaurantClient.GetRankedAsync(strategy))?.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading rankings: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public string GetPercentage(int value, int total)
    {
        if (total == 0) return "0";
        return ((value * 100.0) / total).ToString("F1");
    }

    public int GetReviewCount(Guid restaurantId)
    {
        return _allReviews?.Count(r => r.RestaurantId == restaurantId) ?? 0;
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

    public async ValueTask DisposeAsync()
    {
        _realTimeService.OnRankingsUpdated -= HandleRankingsUpdate;
        await _realTimeService.DisposeAsync();
    }

    public class RestaurantTypeStat
    {
        public string Label { get; init; } = "";
        public int Count { get; init; }
        public string Gradient { get; init; } = "";
        public int BarHeight { get; init; } = 8;
    }
}

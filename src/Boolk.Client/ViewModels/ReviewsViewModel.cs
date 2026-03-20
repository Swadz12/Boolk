using Boolk.Application.DTOs;
using Boolk.Client.ApiClients;
using Boolk.Client.Services;

namespace Boolk.Client.ViewModels;

public class ReviewsViewModel : ViewModelBase, IAsyncDisposable
{
    private readonly RestaurantApiClient _restaurantClient;
    private readonly ReviewApiClient _reviewClient;
    private readonly AuthApiClient _authClient;
    private readonly RankingRealTimeService _realTimeService;

    public ReviewsViewModel(
        RestaurantApiClient restaurantClient,
        ReviewApiClient reviewClient,
        AuthApiClient authClient,
        RankingRealTimeService realTimeService)
    {
        _restaurantClient = restaurantClient;
        _reviewClient = reviewClient;
        _authClient = authClient;
        _realTimeService = realTimeService;
    }

    public NewReviewModel NewReview { get; set; } = new();

    private string _selectedRestaurantId = "";
    public string SelectedRestaurantId { get => _selectedRestaurantId; set => SetProperty(ref _selectedRestaurantId, value); }

    private UserDto? _currentUser;
    public UserDto? CurrentUser { get => _currentUser; private set => SetProperty(ref _currentUser, value); }

    private List<RestaurantDto>? _restaurants;
    public List<RestaurantDto>? Restaurants { get => _restaurants; private set => SetProperty(ref _restaurants, value); }

    private List<ReviewDto>? _reviews;
    public List<ReviewDto>? Reviews { get => _reviews; private set => SetProperty(ref _reviews, value); }

    private int _totalReviews;
    public int TotalReviews { get => _totalReviews; private set => SetProperty(ref _totalReviews, value); }

    private double _averagePrice;
    public double AveragePrice { get => _averagePrice; private set => SetProperty(ref _averagePrice, value); }

    private decimal _averageSatiety;
    public decimal AverageSatiety { get => _averageSatiety; private set => SetProperty(ref _averageSatiety, value); }

    private string _message = "";
    public string Message { get => _message; set => SetProperty(ref _message, value); }

    private bool _isSubmitting;
    public bool IsSubmitting { get => _isSubmitting; private set => SetProperty(ref _isSubmitting, value); }

    public Func<Func<Task>, Task>? InvokeAsync { get; set; }

    public async Task InitializeAsync()
    {
        _realTimeService.OnRankingsUpdated += HandleRankingsUpdate;
        CurrentUser = await _authClient.GetCurrentUserAsync();
        await LoadDataAsync();
    }

    public async Task StartSignalRAsync()
    {
        try
        {
            await _realTimeService.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Reviews] SignalR connection failed: {ex.Message}");
        }
    }

    private void HandleRankingsUpdate(IEnumerable<RestaurantDto> rankings)
    {
        if (InvokeAsync != null)
        {
            _ = InvokeAsync(async () =>
            {
                await LoadDataAsync();
                NotifyStateChanged();
            });
        }
    }

    public async Task LoadDataAsync()
    {
        try
        {
            var result = await _restaurantClient.GetAllAsync(1, 1000);
            Restaurants = result?.Items.ToList();
            Reviews = (await _reviewClient.GetAllAsync())?.ToList();
            TotalReviews = Reviews?.Count ?? 0;

            if (Reviews != null && Reviews.Any())
            {
                AveragePrice = Reviews.Average(r => r.Price);
                AverageSatiety = Reviews.Average(r => (decimal)r.SatietyLevel);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
        }
    }

    public async Task SubmitReviewAsync()
    {
        if (CurrentUser == null)
        {
            Message = "You must be logged in to add a review";
            return;
        }

        IsSubmitting = true;
        Message = "";
        NotifyStateChanged();
        try
        {
            if (Guid.TryParse(SelectedRestaurantId, out var restaurantId))
            {
                await _reviewClient.CreateAsync(
                    restaurantId, CurrentUser.Id, NewReview.Price, NewReview.SatietyLevel, NewReview.Comment);

                Message = "Review added successfully!";
                NewReview = new NewReviewModel();
                SelectedRestaurantId = "";

                await LoadDataAsync();

                await Task.Delay(1500);
                Message = "";
            }
            else
            {
                Message = "Invalid Restaurant ID";
            }
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

    public void SetSatiety(int value)
    {
        NewReview.SatietyLevel = Math.Clamp(value, 1, 10);
        NotifyStateChanged();
    }

    public async Task RefreshAsync()
    {
        await LoadDataAsync();
    }

    public string GetRestaurantName(Guid restaurantId)
    {
        var restaurant = Restaurants?.FirstOrDefault(r => r.Id == restaurantId);
        return restaurant != null ? $"{restaurant.Name} - {restaurant.City}" : "Unknown Restaurant";
    }

    public async ValueTask DisposeAsync()
    {
        _realTimeService.OnRankingsUpdated -= HandleRankingsUpdate;
        await _realTimeService.DisposeAsync();
    }

    public class NewReviewModel
    {
        public double Price { get; set; } = 0;
        public int SatietyLevel { get; set; } = 5;
        public string Comment { get; set; } = "";
    }
}

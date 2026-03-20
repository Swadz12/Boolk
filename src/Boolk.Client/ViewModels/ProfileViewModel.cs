using Boolk.Application.DTOs;
using Boolk.Client.ApiClients;
using Boolk.Client.Auth;

namespace Boolk.Client.ViewModels;

public class ProfileViewModel : ViewModelBase
{
    private readonly AuthApiClient _authClient;
    private readonly ReviewApiClient _reviewClient;
    private readonly RestaurantApiClient _restaurantClient;
    private readonly JwtAuthenticationStateProvider _authStateProvider;

    public ProfileViewModel(
        AuthApiClient authClient,
        ReviewApiClient reviewClient,
        RestaurantApiClient restaurantClient,
        JwtAuthenticationStateProvider authStateProvider)
    {
        _authClient = authClient;
        _reviewClient = reviewClient;
        _restaurantClient = restaurantClient;
        _authStateProvider = authStateProvider;
    }

    private bool _isInitialized;
    public bool IsInitialized { get => _isInitialized; private set => SetProperty(ref _isInitialized, value); }

    private UserDto? _currentUser;
    public UserDto? CurrentUser { get => _currentUser; private set => SetProperty(ref _currentUser, value); }

    private List<ReviewDto>? _userReviews;
    public List<ReviewDto>? UserReviews { get => _userReviews; private set => SetProperty(ref _userReviews, value); }

    private List<RestaurantDto>? _restaurants;
    public List<RestaurantDto>? Restaurants { get => _restaurants; private set => SetProperty(ref _restaurants, value); }

    public Func<string, Task<bool>>? ConfirmAction { get; set; }

    public async Task InitializeAsync()
    {
        CurrentUser = await _authClient.GetCurrentUserAsync();
        IsInitialized = true;

        if (CurrentUser != null)
        {
            UserReviews = (await _reviewClient.GetByUserAsync(CurrentUser.Id))?.ToList();
            var result = await _restaurantClient.GetAllAsync(1, 1000);
            Restaurants = result?.Items.ToList();
        }
    }

    public async Task LogoutAsync()
    {
        await _authClient.LogoutAsync();
        _authStateProvider.NotifyAuthenticationStateChanged();
    }

    public async Task DeleteReviewAsync(Guid reviewId)
    {
        if (ConfirmAction != null)
        {
            bool confirmed = await ConfirmAction("Are you sure you want to delete this review?");
            if (!confirmed) return;
        }

        await _reviewClient.DeleteAsync(reviewId);

        if (CurrentUser != null)
        {
            UserReviews = (await _reviewClient.GetByUserAsync(CurrentUser.Id))?.ToList();
        }
    }

    public string GetRestaurantName(Guid restaurantId)
    {
        var restaurant = Restaurants?.FirstOrDefault(r => r.Id == restaurantId);
        return restaurant != null ? $"{restaurant.Name} - {restaurant.City}" : "Unknown Restaurant";
    }
}

using Blazored.LocalStorage;
using Boolk.Application.DTOs;

namespace Boolk.Client.ApiClients;

/// <summary>
/// HTTP client for review API calls.
/// </summary>
public class ReviewApiClient : ApiClientBase
{
    public ReviewApiClient(HttpClient http, ILocalStorageService localStorage) 
        : base(http, localStorage) { }

    /// <summary>
    /// Get all reviews.
    /// </summary>
    public async Task<IEnumerable<ReviewDto>?> GetAllAsync()
    {
        return await GetAsync<IEnumerable<ReviewDto>>("api/v1/reviews");
    }

    /// <summary>
    /// Get reviews for a specific restaurant.
    /// </summary>
    public async Task<IEnumerable<ReviewDto>?> GetByRestaurantAsync(Guid restaurantId)
    {
        return await GetAsync<IEnumerable<ReviewDto>>($"api/v1/reviews/restaurant/{restaurantId}");
    }

    /// <summary>
    /// Get reviews by a specific user.
    /// </summary>
    public async Task<IEnumerable<ReviewDto>?> GetByUserAsync(Guid userId)
    {
        return await GetAsync<IEnumerable<ReviewDto>>($"api/v1/reviews/user/{userId}");
    }

    /// <summary>
    /// Create a new review.
    /// </summary>
    public async Task<ReviewDto?> CreateAsync(Guid restaurantId, Guid userId, double price, int satietyLevel, string comment)
    {
        var request = new CreateReviewRequest(restaurantId, price, satietyLevel, comment);
        return await PostAsync<CreateReviewRequest, ReviewDto>("api/v1/reviews", request);
    }

    /// <summary>
    /// Delete a review.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        await base.DeleteAsync($"api/v1/reviews/{id}");
    }
}

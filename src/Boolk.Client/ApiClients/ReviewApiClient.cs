using Blazored.LocalStorage;
using Boolk.Application.DTOs;

namespace Boolk.Client.ApiClients;

public class ReviewApiClient : ApiClientBase
{
    public ReviewApiClient(HttpClient http, ILocalStorageService localStorage) 
        : base(http, localStorage) { }

    public async Task<IEnumerable<ReviewDto>?> GetAllAsync()
    {
        return await GetAsync<IEnumerable<ReviewDto>>("api/v1/reviews");
    }

    public async Task<IEnumerable<ReviewDto>?> GetByRestaurantAsync(Guid restaurantId)
    {
        return await GetAsync<IEnumerable<ReviewDto>>($"api/v1/reviews/restaurant/{restaurantId}");
    }

    public async Task<IEnumerable<ReviewDto>?> GetByUserAsync(Guid userId)
    {
        return await GetAsync<IEnumerable<ReviewDto>>($"api/v1/reviews/user/{userId}");
    }

    public async Task<ReviewDto?> CreateAsync(Guid restaurantId, Guid userId, double price, int satietyLevel, string comment)
    {
        var request = new CreateReviewRequest(restaurantId, price, satietyLevel, comment);
        return await PostAsync<CreateReviewRequest, ReviewDto>("api/v1/reviews", request);
    }

    public async Task DeleteAsync(Guid id)
    {
        await DeleteAsync($"api/v1/reviews/{id}");
    }
}

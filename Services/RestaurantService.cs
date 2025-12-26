using Boolk.Models;
using Boolk.Repositories.Interfaces;
using Boolk.Factory;
using Boolk.RankingEngine;
using Boolk.RankingEngine.Interfaces;

namespace Boolk.Services;

public class RestaurantService
{
    private readonly IRestaurantRepository _restaurantRepo;
    private readonly IReviewRepository _reviewRepo;
    private readonly RestaurantFactory _factory;
    private readonly RankingService _rankingService;

    public RestaurantService(
        IRestaurantRepository restaurantRepo,
        IReviewRepository reviewRepo,
        RestaurantFactory factory,
        RankingService rankingService)
    {
        _restaurantRepo = restaurantRepo;
        _reviewRepo = reviewRepo;
        _factory = factory;
        _rankingService = rankingService;
    }

    public async Task<RestaurantBase> CreateRestaurant(string type, string name, string city)
    {
        var restaurant = _factory.Create(type, name, city);
        return await _restaurantRepo.CreateAsync(restaurant);
    }

    public async Task<Review> AddReview(Review review)
    {
        return await _reviewRepo.CreateAsync(review);
    }

    public async Task<List<RestaurantBase>> GetRankedRestaurants(IRankingStrategy strategy)
    {
        _rankingService.SetStrategy(strategy);
        
        var restaurants = (await _restaurantRepo.GetAllAsync()).ToList();
        var reviews = (await _reviewRepo.GetAllAsync()).ToList();
        
        return _rankingService.GetTopRestaurants(restaurants, reviews);
    }
}


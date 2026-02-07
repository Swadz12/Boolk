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
        var createdRestaurant = await _restaurantRepo.CreateAsync(restaurant);

        _rankingService.NotifyRankingsUpdated();
        return createdRestaurant;
    }

    public async Task<Review> AddReview(Review review)
    {
        var createdReview = await _reviewRepo.CreateAsync(review);
        
        _rankingService.NotifyRankingsUpdated();
        
        return createdReview;
    }

    public async Task<List<RestaurantBase>> GetRankedRestaurants(IRankingStrategy strategy)
    {
        _rankingService.SetStrategy(strategy);
        
        // TODO: Ranking requires all data for now. Future refactor: move ranking to DB or Cloud Functions.
        var restaurants = (await _restaurantRepo.GetAllAsync(0, 10000)).ToList();
        var reviews = (await _reviewRepo.GetAllAsync()).ToList();
        
        return _rankingService.GetTopRestaurants(restaurants, reviews);
    }

    public async Task<List<RestaurantBase>> GetRestaurants(int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;
        return (await _restaurantRepo.GetAllAsync(skip, pageSize)).ToList();
    }
}


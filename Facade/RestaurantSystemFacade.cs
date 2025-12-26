using Boolk.Models;
using Boolk.Services;
using Boolk.RankingEngine.Interfaces;
using Boolk.RankingEngine.Strategies;

namespace Boolk.Facade;

public class RestaurantSystemFacade
{
    private readonly RestaurantService _resService;
    private readonly UserService _userService;

    public RestaurantSystemFacade(RestaurantService resService, UserService userService)
    {
        _resService = resService;
        _userService = userService;
    }

    public async Task<List<RestaurantBase>> GetRankedRestaurants(IRankingStrategy strategy)
    {
        return await _resService.GetRankedRestaurants(strategy);
    }

    public async Task<List<RestaurantBase>> GetTopFastFood()
    {
        var strategy = new BestValueStrategy();
        var allRestaurants = await GetRankedRestaurants(strategy);
        return allRestaurants.Where(r => r is FastFoodRestaurant).ToList();
    }
}


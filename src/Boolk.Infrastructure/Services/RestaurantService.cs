using Boolk.Application.Common;
using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;
using Boolk.Application.Ranking;
using Boolk.Domain.Entities;
using Boolk.Domain.Factories;
using MediatR;
using Boolk.Application.Events;

namespace Boolk.Infrastructure.Services;

/// <summary>
/// Restaurant service implementation using Factory pattern.
/// </summary>
public class RestaurantService : IRestaurantService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RestaurantFactory _factory;
    private readonly IRankingService _rankingService;
    private readonly IMediator _mediator;
    private readonly IMenuApiClient _menuApiClient;

    public RestaurantService(
        IUnitOfWork unitOfWork, 
        RestaurantFactory factory,
        IRankingService rankingService,
        IMediator mediator,
        IMenuApiClient menuApiClient)
    {
        _unitOfWork = unitOfWork;
        _factory = factory;
        _rankingService = rankingService;
        _mediator = mediator;
        _menuApiClient = menuApiClient;
    }

    public async Task<PagedResult<RestaurantDto>> GetAllAsync(int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;
        var restaurants = await _unitOfWork.Restaurants.GetAllAsync(skip, pageSize);
        var totalCount = await _unitOfWork.Restaurants.GetCountAsync();

        return new PagedResult<RestaurantDto>
        {
            Items = restaurants.Select(MapToDto).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<RestaurantDto?> GetByIdAsync(Guid id)
    {
        var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(id);
        return restaurant == null ? null : MapToDto(restaurant);
    }

    public async Task<RestaurantDto> CreateAsync(CreateRestaurantRequest request)
    {
        // Use Factory pattern for restaurant creation
        var restaurant = _factory.Create(request.Type, request.Name, request.City);
        
        // Fetch menu from external API (optional - restaurant created even if this fails)
        Menu? menu = null;
        try
        {
            menu = await _menuApiClient.GetMenuAsync(request.Name, request.City, request.Type);
            if (menu != null)
            {
                menu.RestaurantId = restaurant.Id;
            }
        }
        catch
        {
            // Menu fetch failed - continue without menu (menu is optional)
        }
        
        await _unitOfWork.Restaurants.CreateAsync(restaurant);
        
        await _mediator.Publish(new RankingChangedEvent 
        { 
            RestaurantId = restaurant.Id,
            ChangeType = RankingChangeType.RestaurantCreated 
        });

        return MapToDto(restaurant);
    }

    public async Task UpdateAsync(Guid id, UpdateRestaurantRequest request)
    {
        var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(id);
        
        if (restaurant == null)
            throw new KeyNotFoundException($"Restaurant with ID {id} not found");

        restaurant.Name = request.Name;
        restaurant.City = request.City;
        
        await _unitOfWork.Restaurants.UpdateAsync(restaurant);

        await _mediator.Publish(new RankingChangedEvent 
        { 
            RestaurantId = id,
            ChangeType = RankingChangeType.RestaurantUpdated 
        });
    }

    public async Task DeleteAsync(Guid id)
    {
        await _unitOfWork.Restaurants.DeleteAsync(id);

        await _mediator.Publish(new RankingChangedEvent 
        { 
            RestaurantId = id,
            ChangeType = RankingChangeType.RestaurantDeleted 
        });
    }

    public async Task<IEnumerable<RestaurantDto>> GetRankedAsync(string strategy)
    {
        // Delegate to the ranking service with Strategy pattern
        return await _rankingService.GetRankedRestaurantsAsync(strategy);
    }

    private static RestaurantDto MapToDto(RestaurantBase restaurant)
    {
        return new RestaurantDto(
            restaurant.Id,
            restaurant.Name,
            restaurant.City,
            restaurant.GetType().Name,
            restaurant.DisplayName,
            restaurant.DisplayIcon
        );
    }
}


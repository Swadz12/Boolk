using Boolk.Application.Common;
using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boolk.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RestaurantsController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantsController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    /// <summary>
    /// Get all restaurants with pagination.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<RestaurantDto>>> GetAll(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var result = await _restaurantService.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific restaurant by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RestaurantDto>> GetById(Guid id)
    {
        var result = await _restaurantService.GetByIdAsync(id);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    /// <summary>
    /// Create a new restaurant. Requires authentication.
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<RestaurantDto>> Create([FromBody] CreateRestaurantRequest request)
    {
        var result = await _restaurantService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update an existing restaurant. Requires authentication.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRestaurantRequest request)
    {
        try
        {
            await _restaurantService.UpdateAsync(id, request);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Delete a restaurant. Requires authentication.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _restaurantService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Get ranked restaurants based on strategy.
    /// </summary>
    [HttpGet("ranked")]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRanked(
        [FromQuery] string strategy = "best-value")
    {
        var result = await _restaurantService.GetRankedAsync(strategy);
        return Ok(result);
    }
}

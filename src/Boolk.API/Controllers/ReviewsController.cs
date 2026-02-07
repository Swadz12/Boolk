using System.Security.Claims;
using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boolk.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    /// <summary>
    /// Get all reviews.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll()
    {
        var result = await _reviewService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get reviews for a specific restaurant.
    /// </summary>
    [HttpGet("restaurant/{restaurantId:guid}")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByRestaurant(Guid restaurantId)
    {
        var result = await _reviewService.GetByRestaurantIdAsync(restaurantId);
        return Ok(result);
    }

    /// <summary>
    /// Get reviews by a specific user.
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByUser(Guid userId)
    {
        var result = await _reviewService.GetByUserIdAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific review by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReviewDto>> GetById(Guid id)
    {
        var result = await _reviewService.GetByIdAsync(id);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    /// <summary>
    /// Create a new review. Requires authentication.
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ReviewDto>> Create([FromBody] CreateReviewRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await _reviewService.CreateAsync(userId, request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Delete a review. Requires authentication.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _reviewService.DeleteAsync(id);
        return NoContent();
    }
}

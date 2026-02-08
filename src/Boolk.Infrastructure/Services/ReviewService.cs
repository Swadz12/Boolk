using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;
using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;
using Boolk.Domain.Entities;
using MediatR;
using Boolk.Application.Events;

namespace Boolk.Infrastructure.Services;

/// <summary>
/// Review service implementation.
/// </summary>
public class ReviewService : IReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public ReviewService(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<IEnumerable<ReviewDto>> GetAllAsync()
    {
        var reviews = await _unitOfWork.Reviews.GetAllAsync();
        return reviews.Select(MapToDto);
    }

    public async Task<IEnumerable<ReviewDto>> GetByRestaurantIdAsync(Guid restaurantId)
    {
        var reviews = await _unitOfWork.Reviews.GetByRestaurantIdAsync(restaurantId);
        return reviews.Select(MapToDto);
    }

    public async Task<IEnumerable<ReviewDto>> GetByUserIdAsync(Guid userId)
    {
        var reviews = await _unitOfWork.Reviews.GetByUserIdAsync(userId);
        return reviews.Select(MapToDto);
    }

    public async Task<ReviewDto?> GetByIdAsync(Guid id)
    {
        var review = await _unitOfWork.Reviews.GetByIdAsync(id);
        return review == null ? null : MapToDto(review);
    }

    public async Task<ReviewDto> CreateAsync(Guid userId, CreateReviewRequest request)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        
        var review = new Review
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            UserName = user?.Name ?? "Anonymous",
            RestaurantId = request.RestaurantId,
            Price = request.Price,
            SatietyLevel = request.SatietyLevel,
            Comment = request.Comment,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Reviews.CreateAsync(review);
        
        await _mediator.Publish(new RankingChangedEvent 
        { 
            RestaurantId = review.RestaurantId,
            ChangeType = RankingChangeType.ReviewAdded 
        });

        return MapToDto(review);
    }

    public async Task DeleteAsync(Guid id)
    {
        var review = await _unitOfWork.Reviews.GetByIdAsync(id);
        
        await _unitOfWork.Reviews.DeleteAsync(id);

        if (review != null)
        {
            await _mediator.Publish(new RankingChangedEvent 
            { 
                RestaurantId = review.RestaurantId,
                ChangeType = RankingChangeType.ReviewDeleted 
            });
        }
    }

    private static ReviewDto MapToDto(Review review)
    {
        return new ReviewDto(
            review.Id,
            review.UserId,
            review.UserName,
            review.RestaurantId,
            review.Price,
            review.SatietyLevel,
            review.Comment,
            review.CreatedAt
        );
    }
}

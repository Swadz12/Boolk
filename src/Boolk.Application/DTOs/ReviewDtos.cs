namespace Boolk.Application.DTOs;

/// <summary>
/// DTO for review data sent over the API.
/// </summary>
public record ReviewDto(
    Guid Id,
    Guid UserId,
    string UserName,
    Guid RestaurantId,
    double Price,
    int SatietyLevel,
    string Comment,
    DateTime CreatedAt
);

/// <summary>
/// Request DTO for creating a new review.
/// </summary>
public record CreateReviewRequest(
    Guid RestaurantId,
    double Price,
    int SatietyLevel,
    string Comment
);

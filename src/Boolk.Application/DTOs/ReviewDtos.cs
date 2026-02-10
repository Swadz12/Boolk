namespace Boolk.Application.DTOs;

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

public record CreateReviewRequest(
    Guid RestaurantId,
    double Price,
    int SatietyLevel,
    string Comment
);

namespace Boolk.Application.DTOs;

public record RestaurantDto(
    Guid Id,
    string Name,
    string City,
    string Type,
    string DisplayName,
    string DisplayIcon
);

public record CreateRestaurantRequest(
    string Type,
    string Name,
    string City
);

public record UpdateRestaurantRequest(
    string Name,
    string City
);

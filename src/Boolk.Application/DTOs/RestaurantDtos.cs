namespace Boolk.Application.DTOs;

/// <summary>
/// DTO for restaurant data sent over the API.
/// </summary>
public record RestaurantDto(
    Guid Id,
    string Name,
    string City,
    string Type,
    string DisplayName,
    string DisplayIcon
);

/// <summary>
/// Request DTO for creating a new restaurant.
/// </summary>
public record CreateRestaurantRequest(
    string Type,
    string Name,
    string City
);

/// <summary>
/// Request DTO for updating an existing restaurant.
/// </summary>
public record UpdateRestaurantRequest(
    string Name,
    string City
);

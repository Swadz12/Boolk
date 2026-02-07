namespace Boolk.Application.DTOs;

/// <summary>
/// DTO for user data sent over the API (excludes sensitive data like password hash).
/// </summary>
public record UserDto(
    Guid Id,
    string Email,
    string Name,
    DateTime BirthDate
);

/// <summary>
/// Request DTO for user login.
/// </summary>
public record LoginRequest(
    string Email,
    string Password
);

/// <summary>
/// Request DTO for user registration.
/// </summary>
public record RegisterRequest(
    string Email,
    string Name,
    DateTime BirthDate,
    string Password
);

/// <summary>
/// Response DTO for authentication operations.
/// </summary>
public record AuthResponse(
    bool Success,
    string? Token,
    UserDto? User,
    string? Error
);

namespace Boolk.Application.DTOs;

public record UserDto(
    Guid Id,
    string Email,
    string Name,
    DateTime BirthDate
);

public record LoginRequest(
    string Email,
    string Password
);

public record RegisterRequest(
    string Email,
    string Name,
    DateTime BirthDate,
    string Password
);

public record AuthResponse(
    bool Success,
    string? Token,
    UserDto? User,
    string? Error
);

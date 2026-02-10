using Boolk.Application.DTOs;

namespace Boolk.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<UserDto?> GetCurrentUserAsync(Guid userId);
}

using Boolk.Domain.Entities;

namespace Boolk.Application.Interfaces;

/// <summary>
/// Service interface for JWT token generation and validation.
/// </summary>
public interface IJwtTokenService
{
    string GenerateToken(User user);
    Guid? ValidateToken(string token);
}

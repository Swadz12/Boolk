using Boolk.Domain.Entities;

namespace Boolk.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);

}

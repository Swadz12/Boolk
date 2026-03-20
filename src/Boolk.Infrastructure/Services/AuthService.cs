using System.Security.Cryptography;
using System.Text;
using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;
using Boolk.Domain.Entities;

namespace Boolk.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtService;

    public AuthService(IUnitOfWork unitOfWork, IJwtTokenService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        
        if (user == null)
        {
            return new AuthResponse(false, null, null, "User not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) 
        {
            return new AuthResponse(false, null, null, "Invalid password");
        }

        var token = _jwtService.GenerateToken(user);
        var userDto = new UserDto(user.Id, user.Email, user.Name, user.BirthDate);

        return new AuthResponse(true, token, userDto, null);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        
        if (existingUser != null)
        {
            return new AuthResponse(false, null, null, "User with this email already exists");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Name = request.Name,
            BirthDate = request.BirthDate,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _unitOfWork.Users.CreateAsync(user);

        var token = _jwtService.GenerateToken(user);
        var userDto = new UserDto(user.Id, user.Email, user.Name, user.BirthDate);

        return new AuthResponse(true, token, userDto, null);
    }

    public async Task<UserDto?> GetCurrentUserAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        
        if (user == null) return null;
        
        return new UserDto(user.Id, user.Email, user.Name, user.BirthDate);
    }

}

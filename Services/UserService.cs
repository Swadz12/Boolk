using Boolk.Models;
using Boolk.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Boolk.Services;

public class UserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<User> RegisterUser(string email, string password)
    {
        var existingUser = await _userRepo.GetByEmailAsync(email);
        if (existingUser != null)
            throw new InvalidOperationException("User with this email already exists");

        var passwordHash = HashPassword(password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash
        };

        return await _userRepo.CreateAsync(user);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}


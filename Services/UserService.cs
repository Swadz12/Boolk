using Boolk.Models;
using Boolk.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Boolk.Services;

public class UserService
{
    private readonly IUserRepository _userRepo;
    private readonly ProtectedLocalStorage _localStorage;
    private const string UserKey = "currentUserEmail";

    public User? CurrentUser { get; private set; }

    public UserService(IUserRepository userRepo, ProtectedLocalStorage localStorage)
    {
        _userRepo = userRepo;
        _localStorage = localStorage;
    }

    // Call on app startup to restore session
    public async Task InitializeAsync()
    {
        var result = await _localStorage.GetAsync<string>(UserKey);
        if (result.Success && !string.IsNullOrEmpty(result.Value))
        {
            CurrentUser = await _userRepo.GetByEmailAsync(result.Value);
        }
    }

    public async Task<User> RegisterUser(string email,string name,DateTime birthdate, string password)
    {
        var existingUser = await _userRepo.GetByEmailAsync(email);
        if (existingUser != null)
            throw new InvalidOperationException("User with this email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Name = name,
            BirthDate = birthdate,
            PasswordHash = HashPassword(password)
        };

        CurrentUser = await _userRepo.CreateAsync(user);

        // Save to localStorage
        await _localStorage.SetAsync(UserKey, CurrentUser.Email);

        return CurrentUser;
    }

    public async Task<User> Login(string email, string password)
    {
        var user = await _userRepo.GetByEmailAsync(email);
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (user.PasswordHash != HashPassword(password))
            throw new InvalidOperationException("Invalid password");

        CurrentUser = user;

        // Save to localStorage
        await _localStorage.SetAsync(UserKey, CurrentUser.Email);

        return CurrentUser;
    }

    public async Task Logout()
    {
        CurrentUser = null;
        await _localStorage.DeleteAsync(UserKey);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
}

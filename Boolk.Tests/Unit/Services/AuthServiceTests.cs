using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;
using Boolk.Domain.Entities;
using Boolk.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Xunit;
using System.Security.Cryptography;
using System.Text;

namespace Boolk.Tests.Unit.Services;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IJwtTokenService> _mockJwtService;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockJwtService = new Mock<IJwtTokenService>();

        _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUserRepo.Object);

        _authService = new AuthService(_mockUnitOfWork.Object, _mockJwtService.Object);
    }
    
    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnSuccess()
    {
        // Arrange
        var request = new LoginRequest("test@test.com", "password123");
        var user = new User 
        { 
            Id = Guid.NewGuid(), 
            Email = request.Email, 
            PasswordHash = HashPassword(request.Password),
            Name = "Test User",
            BirthDate = DateTime.Now
        };

        _mockUserRepo.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        _mockJwtService.Setup(s => s.GenerateToken(user)).Returns("valid-token");

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        result.Success.Should().BeTrue();
        result.Token.Should().Be("valid-token");
        result.User.Should().NotBeNull();
        result.User!.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ShouldReturnFailure()
    {
         // Arrange
        var request = new LoginRequest("test@test.com", "wrongpassword");
        var user = new User 
        { 
            Id = Guid.NewGuid(), 
            Email = request.Email, 
            PasswordHash = HashPassword("correctpassword"),
            Name = "Test User"
        };

        _mockUserRepo.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(user);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Invalid password");
    }

    [Fact]
    public async Task RegisterAsync_WithNewEmail_ShouldReturnSuccess()
    {
        // Arrange
        var request = new RegisterRequest("new@test.com", "New User", DateTime.Now.AddYears(-20), "password123");
        
        _mockUserRepo.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync((User?)null);
        _mockJwtService.Setup(s => s.GenerateToken(It.IsAny<User>())).Returns("new-token");

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        result.Success.Should().BeTrue();
        result.Token.Should().Be("new-token");
        _mockUserRepo.Verify(r => r.CreateAsync(It.Is<User>(u => u.Email == request.Email)), Times.Once);
    }
}

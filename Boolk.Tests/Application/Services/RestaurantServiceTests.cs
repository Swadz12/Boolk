using Boolk.Application.DTOs;
using Boolk.Application.Interfaces;
using Boolk.Domain.Entities;
using Boolk.Domain.Factories;
using Boolk.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Boolk.Tests.Application.Services;

public class RestaurantServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRestaurantRepository> _mockRestaurantRepo;
    private readonly RestaurantFactory _factory;
    private readonly Mock<IRankingService> _mockRankingService;
    private readonly RestaurantService _sut; // System Under Test

    public RestaurantServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRestaurantRepo = new Mock<IRestaurantRepository>();
        _factory = new RestaurantFactory();
        _mockRankingService = new Mock<IRankingService>();

        _mockUnitOfWork.Setup(u => u.Restaurants).Returns(_mockRestaurantRepo.Object);

        _sut = new RestaurantService(_mockUnitOfWork.Object, _factory, _mockRankingService.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ShouldCreateRestaurant()
    {
        // Arrange
        var request = new CreateRestaurantRequest("My Pizza", "City", "Italian");

        // Act
        var result = await _sut.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("My Pizza");
        result.City.Should().Be("City");
        result.Type.Should().Be("ItalianRestaurant");

        _mockRestaurantRepo.Verify(r => r.CreateAsync(It.IsAny<RestaurantBase>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRestaurantExists_ShouldReturnDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var restaurant = new ItalianRestaurant { Id = id, Name = "My Pizza", City = "City" };
        _mockRestaurantRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(restaurant);

        // Act
        var result = await _sut.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Name.Should().Be("My Pizza");
    }

    [Fact]
    public async Task GetByIdAsync_WhenRestaurantDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRestaurantRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((RestaurantBase?)null);

        // Act
        var result = await _sut.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }
}

using Boolk.Domain.Entities;
using Boolk.Domain.Factories;
using FluentAssertions;
using Xunit;

namespace Boolk.Tests.Domain;

public class RestaurantFactoryTests
{
    private readonly RestaurantFactory _factory;

    public RestaurantFactoryTests()
    {
        _factory = new RestaurantFactory();
    }

    [Theory]
    [InlineData("fastfood", typeof(FastFoodRestaurant))]
    [InlineData("FastFoodRestaurant", typeof(FastFoodRestaurant))]
    [InlineData("italian", typeof(ItalianRestaurant))]
    [InlineData("sushi", typeof(Sushi))]
    public void CreateFromData_ShouldReturnCorrectType_WhenTypeIsValid(string type, Type expectedType)
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Restaurant";
        var city = "Test City";

        // Act
        var result = _factory.CreateFromData(type, name, city, id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(expectedType);
        result!.Id.Should().Be(id); // Important: Verify ID is preserved
        result.Name.Should().Be(name);
        result.City.Should().Be(city);
    }

    [Fact]
    public void CreateFromData_ShouldReturnNull_WhenTypeIsInvalid()
    {
        // Act
        var result = _factory.CreateFromData("invalid_type", "Name", "City", Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public void Create_ShouldThrowException_WhenTypeIsInvalid()
    {
         // Act
         Action act = () => _factory.Create("invalid_type", "Name", "City");

         // Assert
         act.Should().Throw<ArgumentException>();
    }
}

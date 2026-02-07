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
    [InlineData("Italian", typeof(ItalianRestaurant))]
    [InlineData("Burger", typeof(Burgers))]
    [InlineData("Sushi", typeof(Sushi))]
    [InlineData("Kebab", typeof(Kebab))]
    [InlineData("FastFood", typeof(FastFoodRestaurant))]
    [InlineData("Premium", typeof(PremiumRestaurant))]
    [InlineData("StudentBar", typeof(StudentBar))]
    [InlineData("Asian", typeof(AsianRestaurant))]
    public void CreateRestaurant_WithValidType_ShouldReturnCorrectInstance(string type, Type expectedType)
    {
        // Act
        var restaurant = _factory.CreateRestaurant(type);

        // Assert
        restaurant.Should().NotBeNull();
        restaurant.Should().BeOfType(expectedType);
        restaurant.Type.Should().Be(type);
    }

    [Fact]
    public void CreateRestaurant_WithInvalidType_ShouldThrowException()
    {
        // Act
        Action act = () => _factory.CreateRestaurant("UnknownType");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid restaurant type");
    }
}

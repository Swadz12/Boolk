using Boolk.Application.Ranking;
using Boolk.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Boolk.Tests.Application.Strategies;

public class RankingStrategyTests
{
    private class TestRestaurant : RestaurantBase
    {

        public override string DisplayName => "Test Restaurant";
        public override string DisplayIcon => "Test Icon";
    }

    [Fact]
    public void CheapestStrategy_ShouldRankByLowestPrice()
    {
        // Arrange
        var strategy = new CheapestStrategy();
        var r1 = new TestRestaurant { Id = Guid.NewGuid(), Name = "Expensive" };
        var r2 = new TestRestaurant { Id = Guid.NewGuid(), Name = "Cheap" };

        var restaurants = new List<RestaurantBase> { r1, r2 };
        var reviews = new List<Review>
        {
            new Review { RestaurantId = r1.Id, Price = 100 },
            new Review { RestaurantId = r2.Id, Price = 10 }
        };

        // Act
        var ranked = strategy.Rank(restaurants, reviews);

        // Assert
        ranked.First().Should().Be(r2);
        ranked.Last().Should().Be(r1);
    }

    [Fact]
    public void MostFillingStrategy_ShouldRankByHighestSatiety()
    {
        // Arrange
        var strategy = new MostFillingStrategy();
        var r1 = new TestRestaurant { Id = Guid.NewGuid(), Name = "Filling" };
        var r2 = new TestRestaurant { Id = Guid.NewGuid(), Name = "Light" };

        var restaurants = new List<RestaurantBase> { r1, r2 };
        var reviews = new List<Review>
        {
            new Review { RestaurantId = r1.Id, SatietyLevel = 10 },
            new Review { RestaurantId = r2.Id, SatietyLevel = 1 }
        };

        // Act
        var ranked = strategy.Rank(restaurants, reviews);

        // Assert
        ranked.First().Should().Be(r1);
        ranked.Last().Should().Be(r2);
    }
}

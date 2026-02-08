using Boolk.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Boolk.Tests.Domain;

public class ReviewTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void Constructor_WithValidPriceAndSatiety_ShouldCreateReview(int value)
    {
        // Arrange
        var comment = "Great food!";
        var userId = Guid.NewGuid();

        // Act
        var review = new Review
        {
            Comment = comment,
            Price = value,
            SatietyLevel = value,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        review.Price.Should().Be(value);
        review.SatietyLevel.Should().Be(value);
        review.Comment.Should().Be(comment);
        review.UserId.Should().Be(userId);
    }
}

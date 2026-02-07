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
    public void Constructor_WithValidRating_ShouldCreateReview(int rating)
    {
        // Arrange
        var content = "Great food!";
        var userId = "user123";
        var userName = "John Doe";

        // Act
        var review = new Review
        {
            Content = content,
            Rating = rating,
            UserId = userId,
            UserName = userName,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        review.Rating.Should().Be(rating);
        review.Content.Should().Be(content);
        review.UserId.Should().Be(userId);
    }
}

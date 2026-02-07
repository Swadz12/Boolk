namespace Boolk.Domain.Entities;

/// <summary>
/// Represents a review of a restaurant by a user.
/// </summary>
public class Review
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid RestaurantId { get; set; }
    public double Price { get; set; }
    public int SatietyLevel { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

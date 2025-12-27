namespace Boolk.Models;

public class Review
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public string UserName { get; set; }
    public Guid RestaurantId { get; set; }
    public double Price { get; set; }
    public int SatietyLevel { get; set; }
    public string Comment { get; set; } = string.Empty;
}


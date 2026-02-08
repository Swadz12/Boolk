namespace Boolk.Domain.Entities;

/// <summary>
/// Represents a restaurant's complete menu.
/// </summary>
public class Menu
{
    public Guid Id { get; set; }
    public Guid RestaurantId { get; set; }
    public string Name { get; set; } = "Main Menu";
    public List<MenuCategory> Categories { get; set; } = new();
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

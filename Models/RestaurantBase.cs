namespace Boolk.Models;

public abstract class RestaurantBase
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}


namespace Boolk.Domain.Entities;

public class MenuItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "PLN";
    public NutritionalInfo Nutrition { get; set; } = new();
    public List<string> Allergens { get; set; } = new();
    public List<string> DietaryTags { get; set; } = new();
    public bool IsAvailable { get; set; } = true;
}

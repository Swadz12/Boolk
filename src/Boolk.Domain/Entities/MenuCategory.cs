namespace Boolk.Domain.Entities;

public class MenuCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public List<MenuItem> Items { get; set; } = new();
}

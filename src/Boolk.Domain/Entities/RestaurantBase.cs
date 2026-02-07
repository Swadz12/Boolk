namespace Boolk.Domain.Entities;

/// <summary>
/// Base class for all restaurant types. Contains common properties.
/// </summary>
public abstract class RestaurantBase
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets the display name for this restaurant type (e.g., "Fast Food", "Premium").
    /// Override in derived classes to provide type-specific names.
    /// </summary>
    public virtual string DisplayName => GetType().Name.Replace("Restaurant", "");
    
    /// <summary>
    /// Gets the icon representing this restaurant type.
    /// Override in derived classes to provide type-specific icons.
    /// </summary>
    public virtual string DisplayIcon => "🍴";
}

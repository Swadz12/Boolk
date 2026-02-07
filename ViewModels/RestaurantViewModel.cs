using Boolk.Models;

namespace Boolk.ViewModels;

public class RestaurantViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string DisplayIcon { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;

    public static RestaurantViewModel FromModel(RestaurantBase model)
    {
        var (icon, displayName) = GetDisplayInfo(model);
        return new RestaurantViewModel
        {
            Id = model.Id,
            Name = model.Name,
            City = model.City,
            Type = model.GetType().Name,
            DisplayIcon = icon,
            DisplayName = displayName
        };
    }

    private static (string Icon, string Name) GetDisplayInfo(RestaurantBase restaurant)
    {
        return restaurant switch
        {
            FastFoodRestaurant => ("🍟", "Fast Food"),
            StudentBar => ("🍺", "Student Bar"),
            PremiumRestaurant => ("⭐", "Premium"),
            AsianRestaurant => ("🍜", "Asian"),
            Burgers => ("🍔", "Burgers"),
            Sushi => ("🍱", "Sushi"),
            ItalianRestaurant => ("🍕", "Italian"),
            Kebab => ("🫔", "Kebab"),
            _ => ("🍽️", restaurant.GetType().Name)
        };
    }
}

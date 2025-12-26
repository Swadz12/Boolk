using Boolk.Models;

namespace Boolk.Factory;

public class RestaurantFactory
{
    public RestaurantBase Create(string type, string name, string city)
    {
        return type.ToLower() switch
        {
            "fastfood" => new FastFoodRestaurant { Id = Guid.NewGuid(), Name = name, City = city },
            "studentbar" => new StudentBar { Id = Guid.NewGuid(), Name = name, City = city },
            "premium" => new PremiumRestaurant { Id = Guid.NewGuid(), Name = name, City = city },
            _ => throw new ArgumentException($"Unknown restaurant type: {type}")
        };
    }
}


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
            "asian" => new AsianRestaurant(){ Id = Guid.NewGuid(), Name = name, City = city },
            "burgers" => new Burgers(){ Id = Guid.NewGuid(), Name = name, City = city },
            "kebab" => new Kebab(){ Id = Guid.NewGuid(), Name = name, City = city },
            "italian" => new ItalianRestaurant(){ Id = Guid.NewGuid(), Name = name, City = city },
            "sushi" => new Sushi { Id = Guid.NewGuid(), Name = name, City = city },
            _ => throw new ArgumentException($"Unknown restaurant type: {type}")
        };
    }
}


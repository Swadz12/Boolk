using Boolk.Domain.Entities;

namespace Boolk.Domain.Factories;

public class RestaurantFactory
{
    private readonly Dictionary<string, Func<string, string, RestaurantBase>> _creators = new();

    public RestaurantFactory()
    {
        Register("fastfood", (name, city) => new FastFoodRestaurant { Id = Guid.NewGuid(), Name = name, City = city });
        Register("fastfoodrestaurant", (name, city) => new FastFoodRestaurant { Id = Guid.NewGuid(), Name = name, City = city });
        Register("studentbar", (name, city) => new StudentBar { Id = Guid.NewGuid(), Name = name, City = city });
        Register("premium", (name, city) => new PremiumRestaurant { Id = Guid.NewGuid(), Name = name, City = city });
        Register("premiumrestaurant", (name, city) => new PremiumRestaurant { Id = Guid.NewGuid(), Name = name, City = city });
        Register("asian", (name, city) => new AsianRestaurant { Id = Guid.NewGuid(), Name = name, City = city });
        Register("asianrestaurant", (name, city) => new AsianRestaurant { Id = Guid.NewGuid(), Name = name, City = city });
        Register("burgers", (name, city) => new Burgers { Id = Guid.NewGuid(), Name = name, City = city });
        Register("kebab", (name, city) => new Kebab { Id = Guid.NewGuid(), Name = name, City = city });
        Register("italian", (name, city) => new ItalianRestaurant { Id = Guid.NewGuid(), Name = name, City = city });
        Register("italianrestaurant", (name, city) => new ItalianRestaurant { Id = Guid.NewGuid(), Name = name, City = city });
        Register("sushi", (name, city) => new Sushi { Id = Guid.NewGuid(), Name = name, City = city });
    }

    public void Register(string type, Func<string, string, RestaurantBase> creator)
    {
        _creators[type.ToLower()] = creator;
    }

    public RestaurantBase? CreateFromData(string type, string name, string city, Guid id)
    {
        var normalizedType = type.ToLower().Replace("restaurant", "");
        
        if (_creators.TryGetValue(normalizedType, out var creator) || 
            _creators.TryGetValue(type.ToLower(), out creator))
        {
            var restaurant = creator(name, city);
            restaurant.Id = id; 
            return restaurant;
        }
        
        return null;
    }

    public RestaurantBase Create(string type, string name, string city)
    {
        if (_creators.TryGetValue(type.ToLower(), out var creator))
        {
            return creator(name, city);
        }

        throw new ArgumentException($"Unknown restaurant type: {type}. Available types: {string.Join(", ", _creators.Keys)}");
    }

    public IEnumerable<string> GetRegisteredTypes() => _creators.Keys;
}

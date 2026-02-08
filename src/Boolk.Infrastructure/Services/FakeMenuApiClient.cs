using Boolk.Application.Interfaces;
using Boolk.Domain.Entities;

namespace Boolk.Infrastructure.Services;

/// <summary>
/// Fake menu API client for development and testing.
/// Returns realistic mock data based on restaurant type.
/// </summary>
public class FakeMenuApiClient : IMenuApiClient
{
    private readonly Dictionary<string, Func<Menu>> _menuGenerators;

    public FakeMenuApiClient()
    {
        _menuGenerators = new Dictionary<string, Func<Menu>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Asian"] = GenerateAsianMenu,
            ["Italian"] = GenerateItalianMenu,
            ["FastFood"] = GenerateFastFoodMenu,
            ["Premium"] = GeneratePremiumMenu,
            ["Sushi"] = GenerateSushiMenu,
            ["Kebab"] = GenerateKebabMenu,
            ["Burgers"] = GenerateBurgersMenu
        };
    }

    public async Task<Menu?> GetMenuAsync(string restaurantName, string city, string restaurantType)
    {
        // Simulate network delay
        await Task.Delay(50);
        
        if (_menuGenerators.TryGetValue(restaurantType, out var generator))
        {
            var menu = generator();
            menu.Id = Guid.NewGuid();
            return menu;
        }
        
        return GenerateGenericMenu();
    }

    private static Menu GenerateAsianMenu() => new()
    {
        Name = "Asian Cuisine",
        Categories = new List<MenuCategory>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Noodles",
                DisplayOrder = 1,
                Items = new List<MenuItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Pad Thai",
                        Description = "Rice noodles with shrimp, tofu, peanuts",
                        Price = 32.00m,
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "350g",
                            ServingWeightGrams = 350,
                            Calories = 550,
                            Protein = 22,
                            Carbohydrates = 65,
                            Fat = 18,
                            SaturatedFat = 3.5m,
                            Sugar = 8,
                            Fiber = 4,
                            Sodium = 980
                        },
                        Allergens = new List<string> { "peanuts", "shellfish", "soy" }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Ramen",
                        Description = "Japanese noodle soup with pork and egg",
                        Price = 38.00m,
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "450g",
                            ServingWeightGrams = 450,
                            Calories = 620,
                            Protein = 28,
                            Carbohydrates = 72,
                            Fat = 22,
                            SaturatedFat = 6,
                            Sugar = 4,
                            Fiber = 3,
                            Sodium = 1450
                        },
                        Allergens = new List<string> { "gluten", "egg", "soy" }
                    }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Rice Dishes",
                DisplayOrder = 2,
                Items = new List<MenuItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Fried Rice with Chicken",
                        Description = "Wok-fried rice with vegetables and chicken",
                        Price = 28.00m,
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "400g",
                            ServingWeightGrams = 400,
                            Calories = 580,
                            Protein = 24,
                            Carbohydrates = 78,
                            Fat = 16,
                            SaturatedFat = 3,
                            Sugar = 5,
                            Fiber = 4,
                            Sodium = 890
                        },
                        Allergens = new List<string> { "soy", "egg" }
                    }
                }
            }
        }
    };

    private static Menu GenerateItalianMenu() => new()
    {
        Name = "Italian Cuisine",
        Categories = new List<MenuCategory>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Pasta",
                DisplayOrder = 1,
                Items = new List<MenuItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Spaghetti Carbonara",
                        Description = "Classic Roman pasta with egg, pecorino, and guanciale",
                        Price = 36.00m,
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "320g",
                            ServingWeightGrams = 320,
                            Calories = 680,
                            Protein = 26,
                            Carbohydrates = 58,
                            Fat = 38,
                            SaturatedFat = 14,
                            Sugar = 3,
                            Fiber = 2,
                            Sodium = 920
                        },
                        Allergens = new List<string> { "gluten", "egg", "dairy" }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Penne Arrabbiata",
                        Description = "Spicy tomato sauce with garlic and chili",
                        Price = 28.00m,
                        DietaryTags = new List<string> { "vegan" },
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "300g",
                            ServingWeightGrams = 300,
                            Calories = 420,
                            Protein = 12,
                            Carbohydrates = 72,
                            Fat = 10,
                            SaturatedFat = 1.5m,
                            Sugar = 8,
                            Fiber = 5,
                            Sodium = 680
                        },
                        Allergens = new List<string> { "gluten" }
                    }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Pizza",
                DisplayOrder = 2,
                Items = new List<MenuItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Margherita",
                        Description = "Tomato, mozzarella, fresh basil",
                        Price = 32.00m,
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "1 pizza (300g)",
                            ServingWeightGrams = 300,
                            Calories = 750,
                            Protein = 28,
                            Carbohydrates = 88,
                            Fat = 32,
                            SaturatedFat = 14,
                            Sugar = 6,
                            Fiber = 4,
                            Sodium = 1200
                        },
                        Allergens = new List<string> { "gluten", "dairy" }
                    }
                }
            }
        }
    };

    private static Menu GenerateFastFoodMenu() => new()
    {
        Name = "Fast Food",
        Categories = new List<MenuCategory>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Burgers",
                DisplayOrder = 1,
                Items = new List<MenuItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Classic Cheeseburger",
                        Description = "Beef patty with cheese, lettuce, tomato, pickles",
                        Price = 22.00m,
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "220g",
                            ServingWeightGrams = 220,
                            Calories = 540,
                            Protein = 28,
                            Carbohydrates = 42,
                            Fat = 30,
                            SaturatedFat = 12,
                            Sugar = 8,
                            Fiber = 2,
                            Sodium = 980
                        },
                        Allergens = new List<string> { "gluten", "dairy" }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Double Bacon Burger",
                        Description = "Two beef patties with bacon and cheddar",
                        Price = 32.00m,
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "340g",
                            ServingWeightGrams = 340,
                            Calories = 920,
                            Protein = 52,
                            Carbohydrates = 48,
                            Fat = 58,
                            SaturatedFat = 24,
                            Sugar = 10,
                            Fiber = 2,
                            Sodium = 1650
                        },
                        Allergens = new List<string> { "gluten", "dairy" }
                    }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Sides",
                DisplayOrder = 2,
                Items = new List<MenuItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "French Fries",
                        Description = "Crispy golden fries",
                        Price = 12.00m,
                        DietaryTags = new List<string> { "vegan" },
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "150g",
                            ServingWeightGrams = 150,
                            Calories = 380,
                            Protein = 4,
                            Carbohydrates = 48,
                            Fat = 20,
                            SaturatedFat = 3,
                            Sugar = 0,
                            Fiber = 4,
                            Sodium = 280
                        },
                        Allergens = new List<string>()
                    }
                }
            }
        }
    };

    private static Menu GeneratePremiumMenu() => new()
    {
        Name = "Premium Dining",
        Categories = new List<MenuCategory>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Main Courses",
                DisplayOrder = 1,
                Items = new List<MenuItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Grilled Salmon",
                        Description = "Atlantic salmon with herb butter and vegetables",
                        Price = 68.00m,
                        DietaryTags = new List<string> { "keto", "gluten-free" },
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "280g",
                            ServingWeightGrams = 280,
                            Calories = 480,
                            Protein = 42,
                            Carbohydrates = 8,
                            Fat = 32,
                            SaturatedFat = 8,
                            Sugar = 2,
                            Fiber = 3,
                            Sodium = 520
                        },
                        Allergens = new List<string> { "fish", "dairy" }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Beef Tenderloin",
                        Description = "Prime beef with truffle sauce",
                        Price = 98.00m,
                        DietaryTags = new List<string> { "keto" },
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "250g",
                            ServingWeightGrams = 250,
                            Calories = 520,
                            Protein = 48,
                            Carbohydrates = 4,
                            Fat = 34,
                            SaturatedFat = 14,
                            Sugar = 1,
                            Fiber = 0,
                            Sodium = 680
                        },
                        Allergens = new List<string> { "dairy" }
                    }
                }
            }
        }
    };

    private static Menu GenerateSushiMenu() => new()
    {
        Name = "Sushi & Sashimi",
        Categories = new List<MenuCategory>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Sushi Rolls",
                DisplayOrder = 1,
                Items = new List<MenuItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "California Roll",
                        Description = "Crab, avocado, cucumber (8 pcs)",
                        Price = 28.00m,
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "8 pieces",
                            ServingWeightGrams = 180,
                            Calories = 320,
                            Protein = 12,
                            Carbohydrates = 42,
                            Fat = 12,
                            SaturatedFat = 2,
                            Sugar = 6,
                            Fiber = 3,
                            Sodium = 720
                        },
                        Allergens = new List<string> { "shellfish", "soy" }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Salmon Nigiri",
                        Description = "Fresh salmon on rice (2 pcs)",
                        Price = 18.00m,
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "2 pieces",
                            ServingWeightGrams = 60,
                            Calories = 140,
                            Protein = 10,
                            Carbohydrates = 18,
                            Fat = 4,
                            SaturatedFat = 1,
                            Sugar = 2,
                            Fiber = 0,
                            Sodium = 280
                        },
                        Allergens = new List<string> { "fish", "soy" }
                    }
                }
            }
        }
    };

    private static Menu GenerateKebabMenu() => new()
    {
        Name = "Kebab House",
        Categories = new List<MenuCategory>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Kebabs",
                DisplayOrder = 1,
                Items = new List<MenuItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Chicken Doner",
                        Description = "Grilled chicken with salad and sauce in pita",
                        Price = 24.00m,
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "350g",
                            ServingWeightGrams = 350,
                            Calories = 580,
                            Protein = 32,
                            Carbohydrates = 52,
                            Fat = 26,
                            SaturatedFat = 6,
                            Sugar = 4,
                            Fiber = 4,
                            Sodium = 1100
                        },
                        Allergens = new List<string> { "gluten" }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Mixed Kebab Plate",
                        Description = "Lamb and chicken with rice and salad",
                        Price = 38.00m,
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "450g",
                            ServingWeightGrams = 450,
                            Calories = 720,
                            Protein = 45,
                            Carbohydrates = 58,
                            Fat = 32,
                            SaturatedFat = 10,
                            Sugar = 6,
                            Fiber = 5,
                            Sodium = 980
                        },
                        Allergens = new List<string>()
                    }
                }
            }
        }
    };

    private static Menu GenerateBurgersMenu() => new()
    {
        Name = "Gourmet Burgers",
        Categories = new List<MenuCategory>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Signature Burgers",
                DisplayOrder = 1,
                Items = new List<MenuItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "The Classic",
                        Description = "Angus beef, cheddar, lettuce, tomato, special sauce",
                        Price = 34.00m,
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "280g",
                            ServingWeightGrams = 280,
                            Calories = 680,
                            Protein = 38,
                            Carbohydrates = 48,
                            Fat = 38,
                            SaturatedFat = 16,
                            Sugar = 8,
                            Fiber = 3,
                            Sodium = 1080
                        },
                        Allergens = new List<string> { "gluten", "dairy" }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Veggie Deluxe",
                        Description = "Plant-based patty with all the fixings",
                        Price = 32.00m,
                        DietaryTags = new List<string> { "vegetarian" },
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "260g",
                            ServingWeightGrams = 260,
                            Calories = 520,
                            Protein = 22,
                            Carbohydrates = 52,
                            Fat = 26,
                            SaturatedFat = 4,
                            Sugar = 10,
                            Fiber = 8,
                            Sodium = 820
                        },
                        Allergens = new List<string> { "gluten", "soy" }
                    }
                }
            }
        }
    };

    private static Menu GenerateGenericMenu() => new()
    {
        Name = "Menu",
        Categories = new List<MenuCategory>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Main Dishes",
                DisplayOrder = 1,
                Items = new List<MenuItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Chef's Special",
                        Description = "Ask your server for today's special",
                        Price = 35.00m,
                        Nutrition = new NutritionalInfo
                        {
                            ServingSize = "1 portion",
                            Calories = 550,
                            Protein = 25,
                            Carbohydrates = 45,
                            Fat = 28,
                            SaturatedFat = 8,
                            Sugar = 6,
                            Fiber = 4,
                            Sodium = 800
                        },
                        Allergens = new List<string>()
                    }
                }
            }
        }
    };
}

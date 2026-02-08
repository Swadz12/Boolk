namespace Boolk.Domain.Entities;

/// <summary>
/// Nutritional information following Schema.org NutritionInformation standards.
/// </summary>
public class NutritionalInfo
{
    // Serving & Energy
    public string ServingSize { get; set; } = "1 portion";
    public int? ServingWeightGrams { get; set; }
    public int Calories { get; set; }
    
    // Macronutrients (grams)
    public decimal Protein { get; set; }
    public decimal Carbohydrates { get; set; }
    public decimal Sugar { get; set; }
    public decimal Fiber { get; set; }
    public decimal Fat { get; set; }
    public decimal SaturatedFat { get; set; }
    
    // Other key values (milligrams)
    public int Sodium { get; set; }
    public int? Cholesterol { get; set; }
    
    // Computed properties
    public decimal CaloriesFromFat => Fat * 9;
    public decimal CaloriesFromProtein => Protein * 4;
    public decimal CaloriesFromCarbs => Carbohydrates * 4;
}

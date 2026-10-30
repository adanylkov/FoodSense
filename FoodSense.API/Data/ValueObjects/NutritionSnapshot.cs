using FoodSense.API.Data.Models;

namespace FoodSense.API.Data.ValueObjects;

/// <summary>
/// Represents a snapshot of nutritional information for a specific quantity of food. To store in user consumption logs, use the <see cref="FoodLogEntry"/> entity instead.
/// </summary>
public record NutritionSnapshot
{
    public double Calories { get; init; }
    public double Protein { get; init; }
    public double Fat { get; init; }
    public double Carbs { get; init; }

    private NutritionSnapshot() { }

    public NutritionSnapshot(double calories, double protein, double fat, double carbs)
    {
        Calories = calories;
        Protein = protein;
        Fat = fat;
        Carbs = carbs;
    }

    public static NutritionSnapshot FromNutrients(Nutrients nutrients, decimal quantity)
    {
        ArgumentNullException.ThrowIfNull(nutrients);

        var ratio = (double)(quantity / 100m);

        return new NutritionSnapshot(
            calories: nutrients.Kcal * ratio,
            protein: nutrients.Proteins * ratio,
            fat: nutrients.Fat * ratio,
            carbs: nutrients.Carbohydrates * ratio
        );

    }
}
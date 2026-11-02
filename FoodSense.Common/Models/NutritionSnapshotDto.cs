namespace FoodSense.Common.Models;

/// <summary>
/// Snapshot of nutrition values for a consumed quantity.
/// Mirrors the shape the API serializes, so the Frontend can read API responses
/// without referencing the API project.
/// </summary>
public sealed record NutritionSnapshotDto
{
    public double Calories { get; init; }
    public double Protein { get; init; }
    public double Fat { get; init; }
    public double Carbs { get; init; }

    public NutritionSnapshotDto() { }

    public NutritionSnapshotDto(double calories, double protein, double fat, double carbs)
    {
        Calories = calories;
        Protein = protein;
        Fat = fat;
        Carbs = carbs;
    }
}

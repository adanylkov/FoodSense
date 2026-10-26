namespace FoodSense.Common.Models;

public class SelectedMealItem
{
    public ProductDto Product { get; set; } = null!;
    public decimal Quantity { get; set; } = 100m; // domyślnie 100g/ml

    // Dynamicznie przeliczane makro na podstawie ilości
    private double Ratio => (double)(Quantity / 100m);

    public double Calories => (Product.Nutrients?.EnergyKcal ?? 0) * Ratio;
    public double Protein => (Product.Nutrients?.Proteins ?? 0) * Ratio;
    public double Fat => (Product.Nutrients?.Fat ?? 0) * Ratio;
    public double Carbs => (Product.Nutrients?.Carbohydrates ?? 0) * Ratio;
}
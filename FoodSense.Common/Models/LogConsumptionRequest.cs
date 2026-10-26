namespace FoodSense.Common.Models;

public record LogConsumptionRequest
{
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public int? PantryItemId { get; set; }
    public MealType MealType { get; init; } = MealType.Lunch;
}
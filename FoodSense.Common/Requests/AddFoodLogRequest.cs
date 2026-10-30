using FoodSense.Common.Models;

namespace FoodSense.Common.Requests;

public record AddFoodLogRequest
{
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public int? PantryItemId { get; set; }
    public MealType MealType { get; init; } = MealType.Lunch;
}
using FoodSense.Common.Models;

namespace FoodSense.Common.Requests;

public record EditFoodLogRequest
{
    public int FoodLogEntryId { get; set; }
    public decimal Quantity { get; set; }
    public MealType MealType { get; init; } = MealType.Lunch;
}
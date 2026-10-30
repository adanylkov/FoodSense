using System.Diagnostics.CodeAnalysis;
using FoodSense.API.Data.ValueObjects;
using FoodSense.Common.Models;

namespace FoodSense.API.Data.Models;

public class FoodLogEntry
{
    public int Id { get; set; }
    public string? UserId { get; set; } 
    public DateTime Date { get; set; } = DateTime.UtcNow.Date;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public MealType MealType { get; set; }
    public required NutritionSnapshot Nutritions { get; init; }
    public decimal ConsumedQuantity { get; set; }

    private FoodLogEntry() { } // For EF Core

    [SetsRequiredMembers]
    public FoodLogEntry(Product product, MealType mealType, NutritionSnapshot nutritions, decimal consumedQuantity)
    {
        Product = product;
        ProductId = product.Id;
        MealType = mealType;
        Nutritions = nutritions;
        ConsumedQuantity = consumedQuantity;
    }
}
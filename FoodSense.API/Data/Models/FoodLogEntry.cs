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
    public decimal ConsumedQuantity { get; set; } 
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double Carbs { get; set; }
}

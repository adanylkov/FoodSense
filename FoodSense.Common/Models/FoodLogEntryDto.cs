namespace FoodSense.Common.Models;

/// <summary>
/// A food log entry as sent by the API. Only the fields the Frontend reads are
/// declared; extra fields in the API response are ignored when deserializing.
/// </summary>
public sealed record FoodLogEntryDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int ProductId { get; set; }
    public FoodLogProductDto Product { get; set; } = new();
    public MealType MealType { get; set; }
    public NutritionSnapshotDto Nutritions { get; set; } = new();
    public decimal ConsumedQuantity { get; set; }
}

/// <summary>
/// Minimal product data (id + name) carried inside a food log entry.
/// </summary>
public sealed record FoodLogProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

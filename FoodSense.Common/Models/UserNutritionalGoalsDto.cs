namespace FoodSense.Common.Models;

/// <summary>
/// Daily nutrition targets as sent by the API.
/// </summary>
public sealed record UserNutritionalGoalsDto
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public decimal TargetCalories { get; set; } = 2000;
    public decimal TargetProtein { get; set; } = 150;
    public decimal TargetFat { get; set; } = 70;
    public decimal TargetCarbs { get; set; } = 200;
}

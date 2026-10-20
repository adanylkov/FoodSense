namespace FoodSense.Common.Models;

public record CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public List<CategoryDto> Children { get; set; } = new List<CategoryDto>();
}
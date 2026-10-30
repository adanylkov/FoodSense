using FoodSense.API.Data.ValueObjects;

namespace FoodSense.API.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Barcode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FrontImageUrl { get; set; } = string.Empty;
        public Nutrients Nutrients { get; set; } = new();
        public ICollection<PantryItem> PantryItems { get; set; } = new List<PantryItem>();
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}

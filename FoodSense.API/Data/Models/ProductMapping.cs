namespace FoodSense.API.Data.Models
{
    public class ProductMapping
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}

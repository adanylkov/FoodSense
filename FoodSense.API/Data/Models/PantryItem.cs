using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FoodSense.API.Data.Models
{
    public class PantryItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public decimal Quantity { get; set; }
        
        public Guid? UserId { get; set; }

        public DateTime AddedAt { get; init; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;

        private PantryItem() { } // For EF Core
        public PantryItem(Product product, decimal quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}

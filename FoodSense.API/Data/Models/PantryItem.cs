using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FoodSense.API.Data.Models
{
    public class PantryItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; } = null!;

        public decimal Quantity { get; set; }
        
        public Guid? UserId { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

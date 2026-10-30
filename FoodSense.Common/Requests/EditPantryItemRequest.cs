namespace FoodSense.Common.Requests;

public record EditPantryItemRequest
{
    public int PantryItemId { get; set; }
    public decimal Quantity { get; set; }
}
namespace FoodSense.Common.Requests;

public record AddPantryItemRequest
{
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
}  
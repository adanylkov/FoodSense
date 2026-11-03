using FoodSense.Common.Models;

namespace FoodSense.Common.Requests;

/// <summary>
/// Adds a product to the pantry, persisting the product itself when it is not in the local
/// catalog yet (<c>Product.Id</c> == 0). This is the deferred-persistence path: searching a
/// product never writes it to the database, adding it to the pantry does.
/// </summary>
public record AddPantryItemWithProductRequest
{
    public ProductDto? Product { get; set; }
    public decimal Quantity { get; set; }
}

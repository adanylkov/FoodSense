using FoodSense.API.Data.Models;
using FoodSense.Common.Requests;

namespace FoodSense.API.Services;

public interface IPantryService
{
    Task<PantryItem> AddPantryItemAsync(AddPantryItemRequest request);

    /// <summary>Adds a pantry item, persisting the product first when it is not in the catalog yet.</summary>
    Task<PantryItem> AddPantryItemWithProductAsync(AddPantryItemWithProductRequest request);

    Task<PantryItem?> UpdatePantryItemAsync(EditPantryItemRequest request);
    Task<bool> RemovePantryItemAsync(int pantryItemId);
}

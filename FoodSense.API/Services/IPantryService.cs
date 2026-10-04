using FoodSense.API.Data.Models;
using FoodSense.Common.Models;

namespace FoodSense.API.Services;

public interface IPantryService
{
    Task<PantryItem> AddOnePantryItemAsync(int productId);
    Task<PantryItem> AddPantryItemAsync(int productId, PantryItem pantryItem);
    Task<PantryItem> AddPantryItemAsync(ProductDto product, PantryItem pantryItem);
    Task<PantryItem?> UpdatePantryItemAsync(int pantryItemId, PantryItem pantryItem);
    Task<PantryItem?> UpdatePantryItemAsync(int productId, int pantryItemId, PantryItem pantryItem);
    Task<bool> RemovePantryItemAsync(int pantryItemId);
    Task<bool> RemovePantryItemAsync(int productId, int pantryItemId);
}
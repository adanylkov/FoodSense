using FoodSense.API.Data.Models;
using FoodSense.Common.Requests;

namespace FoodSense.API.Services;

public interface IPantryService
{
    Task<PantryItem> AddPantryItemAsync(AddPantryItemRequest request);
    Task<PantryItem?> UpdatePantryItemAsync(EditPantryItemRequest request);
    Task<bool> RemovePantryItemAsync(int pantryItemId);
}
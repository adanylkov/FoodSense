using FoodSense.API.Data.Models;
using FoodSense.API.Repositories;
using FoodSense.Common.Models;

namespace FoodSense.API.Services;

public class PantryService(IPantryRepository pantryRepository) : IPantryService
{
    public async Task<PantryItem> AddPantryItemAsync(int productId, PantryItem pantryItem)
    {
        pantryItem.ProductId = productId;
        pantryItem.UserId = null; 
        return await pantryRepository.AddAsync(pantryItem);
    }

    public async Task<PantryItem> AddPantryItemAsync(ProductDto product, PantryItem pantryItem)
    {
        pantryItem.ProductId = product.Id;
        pantryItem.UserId = null;
        return await pantryRepository.AddAsync(pantryItem);
    }

    public async Task<PantryItem?> UpdatePantryItemAsync(int pantryItemId, PantryItem pantryItem)
    {
        pantryItem.Id = pantryItemId;
        var success = await pantryRepository.UpdateAsync(pantryItem);
        return success ? pantryItem : null;
    }

    public async Task<PantryItem?> UpdatePantryItemAsync(int productId, int pantryItemId, PantryItem pantryItem)
    {
        var existing = await pantryRepository.GetByIdAsync(pantryItemId);
        if (existing == null || existing.ProductId != productId) return null;

        pantryItem.Id = pantryItemId;
        pantryItem.ProductId = productId;
        var success = await pantryRepository.UpdateAsync(pantryItem);
        return success ? pantryItem : null;
    }

    public async Task<bool> RemovePantryItemAsync(int pantryItemId)
    {
        return await pantryRepository.DeleteAsync(pantryItemId);
    }

    public async Task<bool> RemovePantryItemAsync(int productId, int pantryItemId)
    {
        var item = await pantryRepository.GetByIdAsync(pantryItemId);
        if (item == null || item.ProductId != productId) return false;

        return await pantryRepository.DeleteAsync(pantryItemId);
    }

    public Task<PantryItem> AddOnePantryItemAsync(int productId)
    {
        var pantryItem = new PantryItem
        {
            ProductId = productId,
            Quantity = 1,
            UserId = null
        };

        return pantryRepository.AddAsync(pantryItem);
    }
}
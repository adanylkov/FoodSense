using FoodSense.API.Data.Models;
using FoodSense.API.Repositories;
using FoodSense.Common.Models;
using FoodSense.Common.Requests;

namespace FoodSense.API.Services;

public class PantryService(IPantryRepository pantryRepository, IProductRepository productRepository, ILogger<PantryService> logger) : IPantryService
{
    public async Task<PantryItem> AddPantryItemAsync(AddPantryItemRequest request)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
        {
            throw new ArgumentException($"Product with ID {request.ProductId} does not exist.");
        }

        var pantryItem = new PantryItem(product, request.Quantity);

        return await pantryRepository.AddAsync(pantryItem);
    }

    public async Task<PantryItem> AddPantryItemAsync(ProductDto product, PantryItem pantryItem)
    {
        pantryItem.ProductId = product.Id;
        pantryItem.UserId = null;
        return await pantryRepository.AddAsync(pantryItem);
    }

    public async Task<PantryItem?> UpdatePantryItemAsync(EditPantryItemRequest request)
    {
        var pantryItem = await pantryRepository.GetByIdAsync(request.PantryItemId);
        if (pantryItem is null)
        {
            logger.LogWarning($"Pantry item with ID {request.PantryItemId} does not exist.");
            return null;
        }

        var product = await productRepository.GetByIdAsync(pantryItem.ProductId);
        if (product is null)
        {
            logger.LogWarning($"Product with ID {pantryItem.ProductId} does not exist.");
            return null;
        }

        var updatedPantryItem = new PantryItem(product, request.Quantity)
        {
            AddedAt = pantryItem.AddedAt,
            UpdatedAt = DateTime.UtcNow
        };

        var success = await pantryRepository.UpdateAsync(updatedPantryItem);
        return success ? pantryItem : null;
    }

    public async Task<bool> RemovePantryItemAsync(int pantryItemId)
    {
        return await pantryRepository.DeleteAsync(pantryItemId);
    }
}
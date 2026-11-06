using FoodSense.API.Data.Models;
using FoodSense.API.Extensions;
using FoodSense.API.Repositories;
using FoodSense.Common.Requests;

namespace FoodSense.API.Services;

public class PantryService(
    IPantryRepository pantryRepository,
    IProductRepository productRepository,
    IProductService productService,
    ILogger<PantryService> logger) : IPantryService
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

    /// <summary>Deferred persistence: a product that is not in the catalog yet (Id == 0) is stored
    /// only now, in the same request that adds it to the pantry.</summary>
    public async Task<PantryItem> AddPantryItemWithProductAsync(AddPantryItemWithProductRequest request)
    {
        if (request.Product is null)
        {
            throw new ArgumentException("Product data is required to add a pantry item.");
        }

        if (request.Quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than 0.");
        }

        var product = await productService.GetOrCreateProductAsync(request.Product);
        var pantryItem = new PantryItem(product, request.Quantity);

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

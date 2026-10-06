using FoodSense.API.Data.Models;
using FoodSense.API.Extensions;
using FoodSense.API.Repositories;

namespace FoodSense.API.Services;

public class ProductService(
    IProductRepository productRepository, 
    IOpenFoodFactsClient foodFactsClient,
    IProductMappingRepository productMappingRepository) : IProductService
{
    public async Task<IEnumerable<Product>> GetProductsAsync(int page, int pageSize, bool hasPantryItems = false)
        => await productRepository.GetAllAsync(page, pageSize, hasPantryItems);

    public async Task<int> GetTotalProductCountAsync(bool hasPantryItems = false)
        => await productRepository.GetTotalCountAsync(hasPantryItems);

    public async Task<Product?> GetProductByIdAsync(int id) 
        => await productRepository.GetByIdAsync(id);

    public async Task<Product?> GetProductByBarcodeOrFetchAsync(string barcode)
    {
        var existingProduct = await productRepository.GetByBarcodeAsync(barcode);
        if (existingProduct != null) return existingProduct;

        var openFoodFactProduct = await foodFactsClient.GetProductByBarcodeAsync(barcode);
        if (openFoodFactProduct == null) return null;

        var newProduct = openFoodFactProduct.MapFromOpenFoodFact(barcode);
        await productRepository.AddAsync(newProduct);
        return newProduct;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        await productRepository.AddAsync(product);
        return product;
    }

    public async Task<Product?> UpdateProductAsync(int id, Product product)
    {
        var existing = await productRepository.GetByIdAsync(id);
        if (existing == null) return null;

        product.Id = id;
        await productRepository.UpdateAsync(product);
        return product; 
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var existing = await productRepository.GetByIdAsync(id);
        if (existing == null) return false;

        await productRepository.DeleteAsync(id);
        return true;
    }

    public async Task<Product?> GetProductByMappingNameAsync(string name)
    {
        var mapping = await productMappingRepository.GetByProductNameAsync(name);
        return mapping?.Product;
    }

    public async Task<IEnumerable<Product?>> GetProductsByMappingNamesAsync(IEnumerable<string> names)
    {
        var mappings = await productMappingRepository.GetByProductNamesAsync(names);
        return mappings.Select(m => m.Product);
    }


    public async Task AddProductMappingAsync(string mappingName, int productId)
    {
        var mapping = new ProductMapping
        {
            ProductName = mappingName,
            ProductId = productId
        };
        await productMappingRepository.AddAsync(mapping);
        await productMappingRepository.SaveChangesAsync();
    }
}
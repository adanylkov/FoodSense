using FoodSense.API.Data.Models;
using FoodSense.API.Extensions;
using FoodSense.API.Repositories;
using FoodSense.API.Services.ProductSearch;
using FoodSense.Common.Models;

namespace FoodSense.API.Services;

public class ProductService(
    IProductRepository productRepository,
    IProductMappingRepository productMappingRepository,
    IEnumerable<IProductSearchSource> searchSources) : IProductService
{
    public async Task<IEnumerable<Product>> GetProductsAsync(int page, int pageSize, bool hasPantryItems = false)
        => await productRepository.GetAllAsync(page, pageSize, hasPantryItems);

    public async Task<int> GetTotalProductCountAsync(bool hasPantryItems = false)
        => await productRepository.GetTotalCountAsync(hasPantryItems);

    public async Task<Product?> GetProductByIdAsync(int id)
        => await productRepository.GetByIdAsync(id);

    /// <summary>Read-only lookup: a product from the local OpenFoodFacts snapshot is returned with
    /// <c>Id</c> = 0 and is not written to the catalog. Persistence happens when the product is
    /// added to the pantry.</summary>
    public async Task<ProductDto?> GetProductByBarcodeAsync(string barcode)
    {
        foreach (var source in searchSources)
        {
            var result = await source.GetByBarcodeAsync(barcode);
            if (result is not null) return result;
        }

        return null;
    }

    /// <summary>Name search across the configured product sources. The product repository source
    /// is registered first and is always enabled; additional sources are configuration-dependent.</summary>
    public async Task<IReadOnlyList<ProductSearchResultDto>> SearchProductsByNameAsync(string name, int limit = 10)
    {
        if (string.IsNullOrWhiteSpace(name)) return [];

        var results = new List<ProductSearchResultDto>(limit);
        foreach (var source in searchSources)
        {
            var sourceResults = await source.SearchAsync(name, limit, CancellationToken.None);
            results.AddRange(sourceResults.Where(result => !IsAlreadyInCatalog(result, results)));
        }

        return results
            .Take(limit)
            .ToList();
    }

    private static bool IsAlreadyInCatalog(ProductSearchResultDto candidate, IEnumerable<ProductSearchResultDto> localResults)
        => localResults.Any(local =>
            (!string.IsNullOrWhiteSpace(candidate.Barcode) && local.Barcode == candidate.Barcode)
            || string.Equals(local.Name, candidate.Name, StringComparison.OrdinalIgnoreCase));

    public async Task<Product> GetOrCreateProductAsync(ProductDto product)
    {
        if (product.Id > 0)
        {
            var persisted = await productRepository.GetByIdAsync(product.Id)
                ?? throw new ArgumentException($"Product with ID {product.Id} does not exist.");
            return persisted;
        }

        var barcode = string.IsNullOrWhiteSpace(product.Barcode) ? null : product.Barcode.Trim();
        if (barcode is not null)
        {
            // The same barcode may have been persisted by another request; reuse it to keep the
            // barcode unique constraint from failing.
            var existingByBarcode = await productRepository.GetByBarcodeAsync(barcode);
            if (existingByBarcode is not null) return existingByBarcode;
        }

        var entity = product.ToProduct();
        entity.Barcode = barcode;
        await productRepository.AddAsync(entity);
        return entity;
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto product)
    {
        var entity = product.ToProduct();
        await productRepository.AddAsync(entity);
        return entity.ToDto();
    }

    public async Task<ProductDto?> UpdateProductAsync(int id, ProductDto product)
    {
        var existing = await productRepository.GetByIdAsync(id);
        if (existing is null) return null;

        var entity = product.ToProduct();
        entity.Id = id;
        await productRepository.UpdateAsync(entity);
        return entity.ToDto();
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var existing = await productRepository.GetByIdAsync(id);
        if (existing is null) return false;

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

    public Task<Product?> GetProductByNameAsync(string name)
        => productRepository.GetByNameAsync(name);

    public Task<List<Product>> GetProductsByNameAsync(string name)
        => productRepository.GetAllByNameAsync(name);
}

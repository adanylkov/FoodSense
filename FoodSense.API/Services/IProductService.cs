using FoodSense.API.Data.Models;
using FoodSense.Common.Models;

namespace FoodSense.API.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync(int page, int pageSize, bool hasPantryItems = false);
    Task<int> GetTotalProductCountAsync(bool hasPantryItems = false);
    Task<Product?> GetProductByIdAsync(int id);

    /// <summary>Read-only barcode lookup. OpenFoodFacts hits are returned unpersisted (<c>Id</c> = 0).</summary>
    Task<ProductDto?> GetProductByBarcodeAsync(string barcode);
    Task<ProductDto?> GetProductByBarcodeAsync(long barcode)
    {
        return GetProductByBarcodeAsync(barcode.ToString());
    }

    /// <summary>Product name search over the local catalog and OpenFoodFacts. Nothing is persisted.</summary>
    Task<IReadOnlyList<ProductSearchResultDto>> SearchProductsByNameAsync(string name, int limit = 10);

    /// <summary>Persists a product that is not in the catalog yet (deferred persistence).</summary>
    Task<Product> GetOrCreateProductAsync(ProductDto product);

    Task<ProductDto> CreateProductAsync(ProductDto product);
    Task<ProductDto?> UpdateProductAsync(int id, ProductDto product);
    Task<bool> DeleteProductAsync(int id);
    Task<Product?> GetProductByMappingNameAsync(string name);
    Task<IEnumerable<Product?>> GetProductsByMappingNamesAsync(IEnumerable<string> names);
    Task AddProductMappingAsync(string mappingName, int productId);
    Task<Product?> GetProductByNameAsync(string name);
    Task<List<Product>> GetProductsByNameAsync(string name);
}

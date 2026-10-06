using FoodSense.API.Data.Models;

namespace FoodSense.API.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync(int page, int pageSize, bool hasPantryItems = false);
    Task<int> GetTotalProductCountAsync(bool hasPantryItems = false);
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product?> GetProductByBarcodeOrFetchAsync(string barcode);
    Task<Product?> GetProductByBarcodeOrFetchAsync(long barcode)
    {
        return GetProductByBarcodeOrFetchAsync(barcode.ToString());
    }
    Task<Product> CreateProductAsync(Product product);
    Task<Product?> UpdateProductAsync(int id, Product product);
    Task<bool> DeleteProductAsync(int id);
    Task<Product?> GetProductByMappingNameAsync(string name);
    Task<IEnumerable<Product?>> GetProductsByMappingNamesAsync(IEnumerable<string> names);
    Task AddProductMappingAsync(string mappingName, int productId);
}
using FoodSense.API.Data.Models;

namespace FoodSense.API.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync(int page, int pageSize);
    Task<int> GetTotalProductCountAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product?> GetProductByBarcodeOrFetchAsync(string barcode);
    Task<Product?> GetProductByBarcodeOrFetchAsync(long barcode)
    {
        return GetProductByBarcodeOrFetchAsync(barcode.ToString());
    }
    Task<Product> CreateProductAsync(Product product);
    Task<Product?> UpdateProductAsync(int id, Product product);
    Task<bool> DeleteProductAsync(int id);
}
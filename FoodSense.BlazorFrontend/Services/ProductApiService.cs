using FoodSense.Common.Models;

namespace FoodSense.BlazorFrontend.Services;

public interface IProductApiService
{
    Task<HttpResponseMessage> CreateProductAsync(ProductDto product);
    Task<HttpResponseMessage> DeleteProductAsync(int id);
    Task<ProductDto?> GetProductByBarcodeAsync(string barcode);
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ApiResponse<IEnumerable<ProductDto>>?> GetProductsAsync(int page, int pageSize, bool hasPantryItems = false);
    Task<HttpResponseMessage> UpdateProductAsync(int id, ProductDto product);
    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync();
    Task<ProductDto?> GetProductByNameAsync(string productName);
    Task<IEnumerable<ProductDto>?> GetProductsByNameAsync(string productName);
}

public class ProductApiService : IProductApiService
{
    private readonly HttpClient _http;

    public ProductApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ApiResponse<IEnumerable<ProductDto>>?> GetProductsAsync(int page, int pageSize, bool hasPantryItems = false)
    {
        return await _http.GetFromJsonAsync<ApiResponse<IEnumerable<ProductDto>>>(
            $"api/Product/GetProducts?page={page}&pageSize={pageSize}&hasPantryItems={hasPantryItems}");
    }

    public async Task<ProductDto?> GetProductByBarcodeAsync(string barcode)
    {
        return await _http.GetFromJsonAsync<ProductDto>($"api/Product/GetProduct/{Uri.EscapeDataString(barcode)}");
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<ProductDto>($"api/Product/GetProductById/{id}");
    }

    public async Task<HttpResponseMessage> CreateProductAsync(ProductDto product)
    {
        return await _http.PostAsJsonAsync("api/Product/CreateProduct", product);
    }

    public async Task<HttpResponseMessage> UpdateProductAsync(int id, ProductDto product)
    {
        return await _http.PutAsJsonAsync($"api/Product/UpdateProduct/{id}", product);
    }

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync()
    {
        return await _http.GetFromJsonAsync<List<CategoryDto>>("api/Category/GetCategories")
            ?? [];
    }

    public async Task<HttpResponseMessage> DeleteProductAsync(int id)
    {
        return await _http.DeleteAsync($"api/Product/DeleteProduct/{id}");
    }


    public Task<ProductDto?> GetProductByNameAsync(string productName)
    {
        return _http.GetFromJsonAsync<ProductDto>($"api/Product/GetProductByName/{Uri.EscapeDataString(productName)}");
    }

    public async Task<IEnumerable<ProductDto>?> GetProductsByNameAsync(string productName)
    {
        return await _http.GetFromJsonAsync<IEnumerable<ProductDto>>($"api/Product/GetProductsByName/{Uri.EscapeDataString(productName)}");
    }
}
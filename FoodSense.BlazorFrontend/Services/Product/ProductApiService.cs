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
    /// <summary>Name search across the local catalog and OpenFoodFacts. Results are candidates only,
    /// nothing is persisted. A candidate with <c>ProductId</c> == 0 is not in the local catalog.</summary>
    Task<IReadOnlyList<ProductSearchResultDto>> SearchProductsAsync(string name, int limit = 10);
    /// <summary>Persists a product candidate when it is not in the catalog yet and returns it with id.</summary>
    Task<ProductDto?> GetOrCreateProductAsync(ProductDto product);
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

    public async Task<IReadOnlyList<ProductSearchResultDto>> SearchProductsAsync(string name, int limit = 10)
    {
        if (string.IsNullOrWhiteSpace(name)) return [];

        var response = await _http.GetFromJsonAsync<ApiResponse<IEnumerable<ProductSearchResultDto>>>(
            $"api/Product/SearchProducts?name={Uri.EscapeDataString(name)}&limit={limit}");

        return response?.Data?.ToList() ?? [];
    }

    public async Task<ProductDto?> GetOrCreateProductAsync(ProductDto product)
    {
        var response = await _http.PostAsJsonAsync("api/Product/GetOrCreateProduct", product);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ProductDto>();
    }
}
using System.Net.Http.Json;
using FoodSense.Common.Models;

namespace FoodSense.Services;

public interface IProductApiService
{
    Task<HttpResponseMessage> CreatePantryItemAsync(int productId, PantryItemDto pantryItem);
    Task<HttpResponseMessage> CreateProductAsync(ProductDto product);
    Task<HttpResponseMessage> DeletePantryItemAsync(int pantryItemId);
    Task<HttpResponseMessage> DeleteProductAsync(int id);
    Task<ProductDto?> GetProductByBarcodeAsync(string barcode);
    Task<ApiResponse<IEnumerable<ProductDto>>?> GetProductsAsync(int page, int pageSize);
    Task<HttpResponseMessage> QuickAddPantryItemAsync(int productId);
    Task<HttpResponseMessage> UpdatePantryItemAsync(int pantryItemId, PantryItemDto pantryItem);
    Task<HttpResponseMessage> UpdateProductAsync(int id, ProductDto product);
}

public class ProductApiService : IProductApiService
{
    private readonly HttpClient _http;

    public ProductApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ApiResponse<IEnumerable<ProductDto>>?> GetProductsAsync(int page, int pageSize)
    {
        return await _http.GetFromJsonAsync<ApiResponse<IEnumerable<ProductDto>>>(
            $"api/Food/GetProducts?page={page}&pageSize={pageSize}");
    }

    public async Task<ProductDto?> GetProductByBarcodeAsync(string barcode)
    {
        return await _http.GetFromJsonAsync<ProductDto>($"api/Food/GetProduct/{Uri.EscapeDataString(barcode)}");
    }

    public async Task<HttpResponseMessage> CreateProductAsync(ProductDto product)
    {
        return await _http.PostAsJsonAsync("api/Food/CreateProduct", product);
    }

    public async Task<HttpResponseMessage> UpdateProductAsync(int id, ProductDto product)
    {
        return await _http.PutAsJsonAsync($"api/Food/UpdateProduct/{id}", product);
    }

    public async Task<HttpResponseMessage> DeleteProductAsync(int id)
    {
        return await _http.DeleteAsync($"api/Food/DeleteProduct/{id}");
    }

    public async Task<HttpResponseMessage> QuickAddPantryItemAsync(int productId)
    {
        return await _http.PostAsync($"api/Pantry/AddProductToPantry/{productId}", null);
    }

    public async Task<HttpResponseMessage> CreatePantryItemAsync(int productId, PantryItemDto pantryItem)
    {
        return await _http.PostAsJsonAsync($"api/Pantry/CreatePantryItem?productId={productId}", pantryItem);
    }

    public async Task<HttpResponseMessage> UpdatePantryItemAsync(int pantryItemId, PantryItemDto pantryItem)
    {
        return await _http.PutAsJsonAsync($"api/Pantry/UpdatePantryItem/{pantryItemId}", pantryItem);
    }

    public async Task<HttpResponseMessage> DeletePantryItemAsync(int pantryItemId)
    {
        return await _http.DeleteAsync($"api/Pantry/DeletePantryItem/{pantryItemId}");
    }
}
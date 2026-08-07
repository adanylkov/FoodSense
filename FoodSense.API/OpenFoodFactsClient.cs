using System.Net.Http.Json;

public interface IOpenFoodFactsClient
{
    Task<ProductDto?> GetProductByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
    Task<ProductDto?> GetProductByBarcodeAsync(long barcode, CancellationToken cancellationToken = default)
    {
        return GetProductByBarcodeAsync(barcode.ToString(), cancellationToken);
    }
}

public class OpenFoodFactsClient : IOpenFoodFactsClient
{
    private readonly HttpClient _httpClient;

    public OpenFoodFactsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProductDto?> GetProductByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        // OpenFoodFacts v2 JSON endpoint
        var response = await _httpClient.GetAsync($"api/v2/product/{barcode}.json", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<OpenFoodFactsResponse>(cancellationToken: cancellationToken);

        // Status 1 indicates product was found
        if (result?.Status == 1 && result.Product != null)
        {
            return result.Product;
        }

        return null;
    }
}
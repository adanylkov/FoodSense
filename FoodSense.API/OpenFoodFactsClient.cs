using System.Net.Http.Json;
using FoodSense.API.Data.Models;

public interface IOpenFoodFactsClient
{
    Task<OpenFoodFactProduct?> GetProductByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
    Task<OpenFoodFactProduct?> GetProductByBarcodeAsync(long barcode, CancellationToken cancellationToken = default)
    {
        return GetProductByBarcodeAsync(barcode.ToString(), cancellationToken);
    }

    /// <summary>Full-text product search. Returns candidates in OpenFoodFacts order, best match first.</summary>
    Task<IReadOnlyList<OpenFoodFactProduct>> SearchProductsAsync(
        string query,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}

public class OpenFoodFactsClient : IOpenFoodFactsClient
{
    // Fields the search endpoint returns for each candidate. image_small_url keeps the dropdown
    // thumbnails light; nutriments comes as an object keyed by *_100g names.
    private const string SearchFields = "code,product_name,brands,quantity,image_small_url,image_url,nutriments";

    private readonly HttpClient _httpClient;

    public OpenFoodFactsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OpenFoodFactProduct?> GetProductByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
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

    public async Task<IReadOnlyList<OpenFoodFactProduct>> SearchProductsAsync(
        string query,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var terms = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (terms.Length == 0) return [];

        // OpenFoodFacts treats | as an OR of search terms, so "nutella dark" matches either word.
        var searchTerms = Uri.EscapeDataString(string.Join("|", terms));
        var url = $"api/v2/search.json?page=1&pageSize={pageSize}&search_terms={searchTerms}&tags_fields={SearchFields}";

        var response = await _httpClient.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return [];
        }

        var result = await response.Content.ReadFromJsonAsync<OpenFoodFactsSearchResponse>(cancellationToken: cancellationToken);
        return result?.Products ?? [];
    }
}

public class OpenFoodFactsSearchResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("count")]
    public int Count { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("products")]
    public IReadOnlyList<OpenFoodFactProduct> Products { get; set; } = [];
}

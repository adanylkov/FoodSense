using System.Net.Http.Json;
using FoodSense.Common.Models;

namespace FoodSense.Services;

public interface IMatchApiService
{
	Task<ApiResponse<IEnumerable<ProductMappingDto>>?> MatchProductsAsync(IEnumerable<string> productNames);
	Task<bool> AddProductMappingAsync(ProductMappingDto mapping);
}

public class MatchApiService : IMatchApiService
{
	private readonly HttpClient _http;
    private readonly ILogger<MatchApiService> _logger;

	public MatchApiService(HttpClient http, ILogger<MatchApiService> logger)
	{
		_http = http;
        _logger = logger;
	}

	public async Task<ApiResponse<IEnumerable<ProductMappingDto>>?> MatchProductsAsync(IEnumerable<string> productNames)
	{
		using var response = await _http.PostAsJsonAsync("api/productmatching/match", productNames);
		if (!response.IsSuccessStatusCode)
		{
            _logger.LogError("Failed to match products. Status code: {StatusCode}, Reason: {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
			return null;
		}

		return await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<ProductMappingDto>>>();
	}

	public async Task<bool> AddProductMappingAsync(ProductMappingDto mapping)
	{
        using var response = await _http.PostAsJsonAsync("api/productmatching/mapping", mapping);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to add product mapping. Status code: {StatusCode}, Reason: {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
        }
        
        return response.IsSuccessStatusCode;
	}

}
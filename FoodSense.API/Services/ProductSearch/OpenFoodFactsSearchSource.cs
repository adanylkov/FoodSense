using FoodSense.API.Extensions;
using FoodSense.Common.Models;

namespace FoodSense.API.Services.ProductSearch;

public sealed class OpenFoodFactsSearchSource(IOpenFoodFactsClient client) : IProductSearchSource
{
    public async Task<ProductDto?> GetByBarcodeAsync(
        string barcode,
        CancellationToken cancellationToken = default)
        => (await client.GetProductByBarcodeAsync(barcode, cancellationToken))?.ToProductDto();

    public async Task<IReadOnlyList<ProductSearchResultDto>> SearchAsync(
        string query,
        int limit,
        CancellationToken cancellationToken = default)
        => (await client.SearchProductsAsync(query, limit, cancellationToken))
            .Select(product => product.ToSearchResult())
            .ToList();
}

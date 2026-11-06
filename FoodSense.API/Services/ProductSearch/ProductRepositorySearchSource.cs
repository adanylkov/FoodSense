using FoodSense.API.Extensions;
using FoodSense.API.Repositories;
using FoodSense.Common.Models;

namespace FoodSense.API.Services.ProductSearch;

public sealed class ProductRepositorySearchSource(IProductRepository productRepository) : IProductSearchSource
{
    public async Task<ProductDto?> GetByBarcodeAsync(
        string barcode,
        CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByBarcodeAsync(barcode);
        return product?.ToDto();
    }

    public async Task<IReadOnlyList<ProductSearchResultDto>> SearchAsync(
        string query,
        int limit,
        CancellationToken cancellationToken = default)
        => (await productRepository.GetAllByNameAsync(query))
            .Take(limit)
            .Select(product => product.ToSearchResult())
            .ToList();
}

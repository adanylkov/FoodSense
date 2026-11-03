using FoodSense.Common.Models;

namespace FoodSense.API.Services.ProductSearch;

public interface IProductSearchSource
{
    Task<ProductDto?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductSearchResultDto>> SearchAsync(
        string query,
        int limit,
        CancellationToken cancellationToken = default);
}

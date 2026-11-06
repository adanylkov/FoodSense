namespace FoodSense.ViewModels;

using FoodSense.BlazorFrontend.Services;
using FoodSense.Common.Models;

/// <summary>
/// Debounced product name search shared by the product form, the calorie tracker and the receipt
/// uploader. Results are candidates only: a candidate with <c>ProductId</c> == 0 is not stored in
/// the local catalog, and it is persisted only when the user explicitly adds it to the pantry or
/// logs a meal.
/// </summary>
public sealed class ProductSearchViewModel(AlertService alerts, IProductApiService productApi)
    : ViewModelBase(alerts)
{
    private const int DebounceMilliseconds = 300;
    private const int ResultLimit = 10;

    private CancellationTokenSource? _debounce;

    public string Query { get; private set; } = string.Empty;

    public IReadOnlyList<ProductSearchResultDto> Results { get; private set; } = [];

    public bool IsSearching { get; private set; }

    public ProductSearchResultDto? SelectedResult { get; private set; }

    /// <summary>True when the dropdown has candidates to show.</summary>
    public bool IsOpen => Results.Count > 0;

    public async Task SearchAsync(string query)
    {
        Query = query?.Trim() ?? string.Empty;

        // Restart the debounce timer so only the last keystroke triggers a request.
        _debounce?.Cancel();
        _debounce = new CancellationTokenSource();
        var token = _debounce.Token;

        if (string.IsNullOrWhiteSpace(Query))
        {
            Results = [];
            NotifyStateChanged();
            return;
        }

        await RunAsync(async () =>
        {
            try
            {
                await Task.Delay(DebounceMilliseconds, token);
                Results = await productApi.SearchProductsAsync(Query, ResultLimit);
            }
            catch (OperationCanceledException)
            {
                // A newer keystroke superseded this request.
            }

            NotifyStateChanged();
        }, busy => IsSearching = busy);
    }

    /// <summary>Remember the picked candidate and collapse the dropdown.</summary>
    public void Select(ProductSearchResultDto result)
    {
        SelectedResult = result;
        Results = [];
        Query = result.Name;
        NotifyStateChanged();
    }

    public void Clear()
    {
        _debounce?.Cancel();
        Query = string.Empty;
        Results = [];
        SelectedResult = null;
        NotifyStateChanged();
    }
}

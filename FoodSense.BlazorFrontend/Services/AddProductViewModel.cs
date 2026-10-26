namespace FoodSense.BlazorFrontend.ViewModels;

using FoodSense.Common.Models;
using FoodSense.ViewModels;
using FoodSense.Services;

public class AddProductModalViewModel : ViewModelBase
{
    private readonly IProductApiService _productApi;

    public bool IsOpen { get; private set; }
    public MealType CurrentMealType { get; private set; } = MealType.Lunch;

    // Pola formularza
    public string SearchQuery { get; set; } = string.Empty;
    public string BarcodeQuery { get; set; } = string.Empty;
    public List<ProductDto> SearchResults { get; private set; } = [];
    public ProductDto? SelectedProduct { get; private set; }
    public decimal Quantity { get; set; } = 100m; // domyślnie 100g/ml

    public event Func<Task>? ProductAdded;

    public AddProductModalViewModel(IProductApiService productApi, AlertService alerts) : base(alerts)
    {
        _productApi = productApi;
    }

    public void Open(MealType mealType)
    {
        IsOpen = true;
        CurrentMealType = mealType;
        SearchQuery = string.Empty;
        BarcodeQuery = string.Empty;
        SearchResults.Clear();
        SelectedProduct = null;
        Quantity = 100m;
        NotifyStateChanged();
    }

    public void Close()
    {
        IsOpen = false;
        NotifyStateChanged();
    }

    public async Task SearchProductsAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            SearchResults.Clear();
            return;
        }

        await RunAsync(async () =>
        {
            // Możesz użyć GetProductsAsync z filtrowaniem lub dedykowanej metody wyszukiwania po nazwie
            var response = await _productApi.GetProductsByNameAsync(SearchQuery.Trim());
            if (response != null)
            {
                SearchResults = response.ToList();        
            }
        });
    }

    public async Task SearchByBarcodeAsync()
    {
        if (string.IsNullOrWhiteSpace(BarcodeQuery)) return;

        await RunAsync(async () =>
        {
            var product = await _productApi.GetProductByBarcodeAsync(BarcodeQuery.Trim());
            if (product != null)
            {
                SelectedProduct = product;
                Alerts.Success = $"Znaleziono produkt: {product.ProductName}";
            }
            else
            {
                Alerts.Error = "Nie znaleziono produktu o takim kodzie kreskowym.";
            }
        });
    }

    public void SelectProduct(ProductDto product)
    {
        SelectedProduct = product;
        SearchResults.Clear();
    }

    public async Task ConfirmConsumptionAsync()
    {
        if (SelectedProduct == null || Quantity <= 0) return;

        await RunAsync(async () =>
        {
            // var response = await _productApi.LogAndConsumeAsync(
            //     productId: SelectedProduct.Id,
            //     quantity: Quantity,
            //     pantryItemId: null, // lub powiązany pantryItem jeśli dostępny
            //     mealType: CurrentMealType
            // );

            // if (response.IsSuccessStatusCode)
            // {
            //     Alerts.Success = "Pomyślnie dodano posiłek!";
            //     Close();
            //     if (ProductAdded != null) await ProductAdded.Invoke();
            // }
            // else
            // {
            //     var error = await response.Content.ReadAsStringAsync();
            //     Alerts.Error = $"Błąd zapisu: {error}";
            // }
        });
    }

    public async Task ClearSelectedProduct()
    {
        SelectedProduct = null;
        Quantity = 100m;
        NotifyStateChanged();
    }
}
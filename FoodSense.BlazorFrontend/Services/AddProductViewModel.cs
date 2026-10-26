namespace FoodSense.BlazorFrontend.ViewModels;

using FoodSense.Common.Models;
using FoodSense.ViewModels;
using FoodSense.Services;
using FoodSense.BlazorFrontend.Services;

public class AddProductModalViewModel : ViewModelBase
{
    private readonly IProductApiService _productApi;
    private readonly ICalorieTrackerApiService _calorieTrackerApi;

    public bool IsOpen { get; private set; }
    public MealType CurrentMealType { get; private set; } = MealType.Lunch;

    // Pola formularza
    public string SearchQuery { get; set; } = string.Empty;
    public string BarcodeQuery { get; set; } = string.Empty;
    public List<ProductDto> SearchResults { get; private set; } = [];
    
    // Lista wielu wybranych produktów do dodania
    public List<SelectedMealItem> SelectedItems { get; private set; } = [];

    // Sumaryczne makro dla całego posiłku
    public double TotalCalories => SelectedItems.Sum(i => i.Calories);
    public double TotalProtein => SelectedItems.Sum(i => i.Protein);
    public double TotalFat => SelectedItems.Sum(i => i.Fat);
    public double TotalCarbs => SelectedItems.Sum(i => i.Carbs);

    public event Func<Task>? ProductAdded;

    public AddProductModalViewModel(IProductApiService productApi, AlertService alerts, ICalorieTrackerApiService calorieTrackerApi) : base(alerts)
    {
        _productApi = productApi;
        _calorieTrackerApi = calorieTrackerApi;
    }

    public void Open(MealType mealType)
    {
        IsOpen = true;
        CurrentMealType = mealType;
        SearchQuery = string.Empty;
        BarcodeQuery = string.Empty;
        SearchResults.Clear();
        SelectedItems.Clear();
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
            NotifyStateChanged();
            return;
        }

        await RunAsync(async () =>
        {
            var response = await _productApi.GetProductsByNameAsync(SearchQuery.Trim());
            if (response != null)
            {
                // Upewniamy się, że pasuje do typu zwracanego przez GetProductsByNameAsync (kolekcja lub pojedynczy obiekt)
                SearchResults = response.ToList();
            }
            else
            {
                SearchResults.Clear();
            }
            NotifyStateChanged();
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
                AddProduct(product);
                BarcodeQuery = string.Empty;
                Alerts.Success = $"Dodano do listy: {product.ProductName}";
            }
            else
            {
                Alerts.Error = "Nie znaleziono produktu o takim kodzie kreskowym.";
            }
            NotifyStateChanged();
        });
    }

    public void AddProduct(ProductDto product)
    {
        var existing = SelectedItems.FirstOrDefault(i => i.Product.Id == product.Id);
        if (existing != null)
        {
            existing.Quantity += 100m; // domyślnie dodaj kolejne 100g
        }
        else
        {
            SelectedItems.Add(new SelectedMealItem { Product = product, Quantity = 100m });
        }

        SearchResults.Clear();
        SearchQuery = string.Empty;
        NotifyStateChanged();
    }

    public void RemoveItem(SelectedMealItem item)
    {
        SelectedItems.Remove(item);
        NotifyStateChanged();
    }

    public async Task ConfirmConsumptionAsync()
    {
        if (!SelectedItems.Any()) return;

        await RunAsync(async () =>
        {
            bool allSuccess = true;

            foreach (var item in SelectedItems)
            {
                var response = await _calorieTrackerApi.LogAndConsumeAsync(
                    productId: item.Product.Id,
                    quantity: item.Quantity,
                    pantryItemId: null,
                    mealType: CurrentMealType
                );

                if (!response.IsSuccessStatusCode)
                {
                    allSuccess = false;
                }
            }

            if (allSuccess)
            {
                Alerts.Success = "Pomyślnie dodano produkty do dziennika!";
                Close();
                if (ProductAdded != null)
                {
                    await ProductAdded.Invoke();
                }
            }
            else
            {
                Alerts.Error = "Wystąpił błąd podczas zapisywania niektórych produktów.";
            }
            NotifyStateChanged();
        });
    }
}
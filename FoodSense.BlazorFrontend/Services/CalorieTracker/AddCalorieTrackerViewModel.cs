namespace FoodSense.BlazorFrontend.ViewModels;

using FoodSense.Common.Models;
using FoodSense.ViewModels;
using FoodSense.BlazorFrontend.Services;

public class AddCalorieTrackerViewModel : ViewModelBase
{
    private readonly IProductApiService _productApi;
    private readonly ICalorieTrackerApiService _calorieTrackerApi;

    public bool IsOpen { get; private set; }
    public MealType CurrentMealType { get; private set; } = MealType.Lunch;

    // Pola formularza
    public string BarcodeQuery { get; set; } = string.Empty;
    
    // Lista wielu wybranych produktów do dodania
    public List<SelectedMealItem> SelectedItems { get; private set; } = [];

    // Sumaryczne makro dla całego posiłku
    public double TotalCalories => SelectedItems.Sum(i => i.Calories);
    public double TotalProtein => SelectedItems.Sum(i => i.Protein);
    public double TotalFat => SelectedItems.Sum(i => i.Fat);
    public double TotalCarbs => SelectedItems.Sum(i => i.Carbs);

    public event Func<Task>? ProductAdded;

    public AddCalorieTrackerViewModel(IProductApiService productApi, AlertService alerts, ICalorieTrackerApiService calorieTrackerApi) : base(alerts)
    {
        _productApi = productApi;
        _calorieTrackerApi = calorieTrackerApi;
    }

    public void Open(MealType mealType)
    {
        IsOpen = true;
        CurrentMealType = mealType;
        BarcodeQuery = string.Empty;
        SelectedItems.Clear();
        NotifyStateChanged();
    }

    public void Close()
    {
        IsOpen = false;
        NotifyStateChanged();
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
        // Unpersisted candidates all have Id == 0, so match on barcode (preferred) or name.
        var existing = SelectedItems.FirstOrDefault(i =>
            (product.Barcode is not null && i.Product.Barcode == product.Barcode)
            || string.Equals(i.Product.ProductName, product.ProductName, StringComparison.OrdinalIgnoreCase));
        if (existing != null)
        {
            existing.Quantity += 100m; // domyślnie dodaj kolejne 100g
        }
        else
        {
            SelectedItems.Add(new SelectedMealItem { Product = product, Quantity = 100m });
        }

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
                // Persistence happens on this explicit "add meal" action, never when a search
                // candidate is picked. Unpersisted candidates come in with Id == 0.
                if (item.Product.Id <= 0)
                {
                    var persisted = await _productApi.GetOrCreateProductAsync(item.Product);
                    if (persisted is null || persisted.Id <= 0)
                    {
                        allSuccess = false;
                        continue;
                    }

                    item.Product = persisted;
                }

                var response = await _calorieTrackerApi.LogConsumptionAsync(
                    new Common.Requests.AddFoodLogRequest
                    {
                        ProductId = item.Product.Id,
                        Quantity = item.Quantity,
                        MealType = CurrentMealType
                    }
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
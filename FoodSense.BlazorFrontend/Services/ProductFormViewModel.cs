namespace FoodSense.ViewModels;

using FoodSense.Common.Models;
using FoodSense.Services;

public class ProductFormViewModel(IProductApiService productApi, AlertService alerts) : ViewModelBase(alerts)
{
    public ProductDto FormModel { get; private set; } = new();
    public IReadOnlyList<CategoryDto> Categories { get; private set; } = [];
    public int? EditingProductId { get; private set; }
    public bool IsSaving { get; private set; }

    // Raised after a successful save so the product list can reload.
    public event Func<Task>? Saved;

    public Task LoadCategoriesAsync() =>
        RunAsync(async () => Categories = await productApi.GetCategoriesAsync());

    public async Task SaveAsync()
    {
        Alerts.Clear();

        await RunAsync(async () =>
        {
            if (EditingProductId is null)
            {
                var response = await productApi.CreateProductAsync(FormModel);
                if (!response.IsSuccessStatusCode)
                {
                    Alerts.Error = $"Failed to create product: {await response.Content.ReadAsStringAsync()}";
                    return;
                }
                Alerts.Success = "Product created successfully.";
            }
            else
            {
                FormModel.Id = EditingProductId.Value;
                var response = await productApi.UpdateProductAsync(EditingProductId.Value, FormModel);
                if (!response.IsSuccessStatusCode)
                {
                    Alerts.Error = $"Failed to update product: {await response.Content.ReadAsStringAsync()}";
                    return;
                }
                Alerts.Success = "Product updated successfully.";
            }

            Reset();
            await Saved.InvokeAllAsync();
        }, busy => IsSaving = busy);
    }

    public async Task LookupBarcodeAsync(string? barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode)) return;

        await RunAsync(async () =>
        {
            var product = await productApi.GetProductByBarcodeAsync(barcode);
            if (product != null) StartEditing(product);
        });
    }

    public void StartEditing(ProductDto product)
    {
        EditingProductId = product.Id;
        FormModel = product.CloneProduct();
        Alerts.Clear();
        NotifyStateChanged();
    }

    public void Reset()
    {
        EditingProductId = null;
        FormModel = new ProductDto { Nutrients = new NutrientsDto() };
        NotifyStateChanged();
    }
}

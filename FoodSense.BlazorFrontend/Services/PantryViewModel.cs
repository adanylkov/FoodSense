namespace FoodSense.ViewModels;

using FoodSense.Common.Models;
using FoodSense.Services;

public class PantryViewModel(IProductApiService productApi, AlertService alerts) : ViewModelBase(alerts)
{
    public ProductDto? ActiveProduct { get; private set; }
    public int? EditingItemId { get; private set; }
    public decimal Quantity { get; set; } = 1;
    public bool IsSaving { get; private set; }

    // Raised after a pantry mutation so the product list can reload.
    public event Func<Task>? PantryChanged;

    public async Task QuickAddAsync(int productId)
    {
        await RunAsync(async () =>
        {
            var response = await productApi.QuickAddPantryItemAsync(productId);
            if (!response.IsSuccessStatusCode)
            {
                Alerts.Error = "Failed to add item to pantry.";
                return;
            }

            Alerts.Success = "Pantry item added.";
            await RaisePantryChangedAsync();
        });
    }

    public async Task SaveAsync()
    {
        if (ActiveProduct is null || Quantity <= 0) return;

        var product = ActiveProduct;
        Alerts.Error = null;

        await RunAsync(async () =>
        {
            var payload = new PantryItemDto
            {
                Id = EditingItemId ?? 0,
                Quantity = Quantity,
                AddedAt = DateTime.UtcNow
            };

            var response = EditingItemId.HasValue
                ? await productApi.UpdatePantryItemAsync(EditingItemId.Value, payload)
                : await productApi.CreatePantryItemAsync(product.Id, payload);

            if (!response.IsSuccessStatusCode)
            {
                Alerts.Error = "Failed to save pantry item.";
                return;
            }

            ResetForm();
            await RaisePantryChangedAsync();
        }, busy => IsSaving = busy);
    }

    public async Task DeleteItemAsync(int pantryItemId)
    {
        await RunAsync(async () =>
        {
            var response = await productApi.DeletePantryItemAsync(pantryItemId);
            if (!response.IsSuccessStatusCode)
            {
                Alerts.Error = "Failed to delete pantry item.";
                return;
            }

            await RaisePantryChangedAsync();
        });
    }

    public void Open(ProductDto product)
    {
        ActiveProduct = product;
        ResetForm();
        NotifyStateChanged();
    }

    public void Close()
    {
        ActiveProduct = null;
        ResetForm();
        NotifyStateChanged();
    }

    public void StartEditingItem(PantryItemDto item)
    {
        EditingItemId = item.Id;
        Quantity = item.Quantity;
        NotifyStateChanged();
    }

    public void ResetForm()
    {
        EditingItemId = null;
        Quantity = 1;
        NotifyStateChanged();
    }

    // Re-resolves the open product from freshly loaded data; closes the modal if it is gone.
    public void RefreshActiveProduct(IEnumerable<ProductDto> products)
    {
        if (ActiveProduct is null) return;
        ActiveProduct = products.FirstOrDefault(p => p.Id == ActiveProduct.Id);
    }

    private Task RaisePantryChangedAsync() => PantryChanged?.Invoke() ?? Task.CompletedTask;
}

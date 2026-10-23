namespace FoodSense.ViewModels;

using FoodSense.Services;

public class ProductDeletionViewModel(IProductApiService productApi, AlertService alerts) : ViewModelBase(alerts)
{
    public int? ProductIdToDelete { get; private set; }
    public bool IsDeleting { get; private set; }

    // Raised after a successful delete so the product list can reload.
    public event Func<Task>? Deleted;

    public void Prompt(int productId)
    {
        ProductIdToDelete = productId;
        NotifyStateChanged();
    }

    public void Cancel()
    {
        ProductIdToDelete = null;
        NotifyStateChanged();
    }

    public async Task ExecuteAsync()
    {
        if (ProductIdToDelete is not { } id) return;

        Alerts.Clear();

        await RunAsync(async () =>
        {
            var response = await productApi.DeleteProductAsync(id);
            if (!response.IsSuccessStatusCode)
            {
                Alerts.Error = $"Failed to delete product: {await response.Content.ReadAsStringAsync()}";
                return;
            }

            Alerts.Success = "Product deleted successfully.";
            ProductIdToDelete = null;
            await Deleted.InvokeAllAsync();
        }, busy => IsDeleting = busy);
    }
}

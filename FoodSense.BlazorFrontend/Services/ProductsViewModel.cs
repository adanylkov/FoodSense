namespace FoodSense.ViewModels;

using FoodSense.Common.Models;
using FoodSense.Services;

public class ProductsViewModel : ViewModelBase
{
    private readonly IProductApiService _productApi;
    private readonly PantryViewModel _pantry;

    public List<ProductDto> Products { get; private set; } = [];
    public ProductDto FormModel { get; set; } = new();
    public IReadOnlyList<CategoryDto> Categories { get; private set; } = [];
    
    public PaginationModel Pagination { get; } = new();
    public bool PantryOnly { get; private set; }

    public int? EditingProductId { get; private set; }
    public int? ProductToDeleteId { get; private set; }

    public bool IsLoading { get; private set; }
    public bool IsSaving { get; private set; }
    
    public string? SuccessMessage => Alerts.Success;
    public string? ErrorMessage => Alerts.Error;

    public ProductsViewModel(IProductApiService productApi, PantryViewModel pantry, AlertService alerts) : base(alerts)
    {
        _productApi = productApi;
        _pantry = pantry;
        _pantry.PantryChanged += () => LoadProductsAsync(Pagination.CurrentPage);
    }

    public async Task LoadProductsAsync(int page)
    {
        ClearAlerts();

        await RunAsync(async () =>
        {
            var response = await _productApi.GetProductsAsync(page, Pagination.PageSize, PantryOnly);
            if (response == null) return;

            Products = response.Data?.ToList() ?? [];
            Pagination.Update(page, response.TotalCount);
            _pantry.RefreshActiveProduct(Products);
        }, busy => IsLoading = busy);
    }

    public Task LoadCategoriesAsync() =>
        RunAsync(async () => Categories = await _productApi.GetCategoriesAsync());

    public async Task SetPantryOnlyAsync(bool pantryOnly)
    {
        if (PantryOnly == pantryOnly) return;

        PantryOnly = pantryOnly;
        await LoadProductsAsync(1);
    }

    public async Task SaveProductAsync()
    {
        ClearAlerts();

        await RunAsync(async () =>
        {
            if (EditingProductId is null)
            {
                var response = await _productApi.CreateProductAsync(FormModel);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    Alerts.Error = $"Failed to create product: {errorMsg}";
                    return;
                }
                Alerts.Success = "Product created successfully.";
            }
            else
            {
                FormModel.Id = EditingProductId.Value;
                var response = await _productApi.UpdateProductAsync(EditingProductId.Value, FormModel);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    Alerts.Error = $"Failed to update product: {errorMsg}";
                    return;
                }
                Alerts.Success = "Product updated successfully.";
            }

            ResetForm();
            await LoadProductsAsync(Pagination.CurrentPage);
        }, busy => IsSaving = busy);
    }

    public async Task HandleBarcodeInputAsync(string? barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode)) return;

        await RunAsync(async () =>
        {
            var product = await _productApi.GetProductByBarcodeAsync(barcode);
            if (product != null) StartEditing(product);
        });
    }

    public async Task ExecuteDeleteAsync()
    {
        if (!ProductToDeleteId.HasValue) return;

        ClearAlerts();

        await RunAsync(async () =>
        {
            await _productApi.DeleteProductAsync(ProductToDeleteId.Value);
            Alerts.Success = "Product deleted successfully.";
            ProductToDeleteId = null;
            await LoadProductsAsync(Pagination.CurrentPage);
        }, busy => IsSaving = busy);
    }

    public void StartEditing(ProductDto product)
    {
        EditingProductId = product.Id;
        FormModel = product.CloneProduct();
        ClearAlerts();
    }

    public void ResetForm()
    {
        EditingProductId = null;
        FormModel = new ProductDto { Nutrients = new NutrientsDto() };
        NotifyStateChanged();
    }

    public void PromptDelete(int id)
    {
        ProductToDeleteId = id;
        NotifyStateChanged();
    }

    public void CancelDelete()
    {
        ProductToDeleteId = null;
        NotifyStateChanged();
    }

    public void ClearAlerts()
    {
        Alerts.Clear();
        NotifyStateChanged();
    }
}

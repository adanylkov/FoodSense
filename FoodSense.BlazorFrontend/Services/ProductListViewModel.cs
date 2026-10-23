namespace FoodSense.ViewModels;

using FoodSense.Common.Models;
using FoodSense.Services;

public class ProductListViewModel : ViewModelBase, IDisposable
{
    private readonly IProductApiService _productApi;
    private readonly PantryViewModel _pantry;
    private readonly ProductFormViewModel _form;
    private readonly ProductDeletionViewModel _deletion;

    public List<ProductDto> Products { get; private set; } = [];
    public PaginationModel Pagination { get; } = new();
    public bool PantryOnly { get; private set; }
    public bool IsLoading { get; private set; }

    public ProductListViewModel(
        IProductApiService productApi,
        PantryViewModel pantry,
        ProductFormViewModel form,
        ProductDeletionViewModel deletion,
        AlertService alerts) : base(alerts)
    {
        _productApi = productApi;
        _pantry = pantry;
        _form = form;
        _deletion = deletion;

        pantry.PantryChanged += ReloadAsync;
        form.Saved += ReloadAsync;
        deletion.Deleted += ReloadAsync;
    }

    public void Dispose()
    {
        _pantry.PantryChanged -= ReloadAsync;
        _form.Saved -= ReloadAsync;
        _deletion.Deleted -= ReloadAsync;
    }

    public Task ReloadAsync() => LoadAsync(Pagination.CurrentPage);

    public async Task LoadAsync(int page)
    {
        Alerts.Clear();

        await RunAsync(async () =>
        {
            var response = await _productApi.GetProductsAsync(page, Pagination.PageSize, PantryOnly);
            if (response == null) return;

            Products = response.Data?.ToList() ?? [];
            Pagination.Update(page, response.TotalCount);
            _pantry.RefreshActiveProduct(Products);
        }, busy => IsLoading = busy);
    }

    public async Task SetPantryOnlyAsync(bool pantryOnly)
    {
        if (PantryOnly == pantryOnly) return;

        PantryOnly = pantryOnly;
        await LoadAsync(1);
    }
}

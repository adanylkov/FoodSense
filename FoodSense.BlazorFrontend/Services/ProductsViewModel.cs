namespace FoodSense.ViewModels;

using FoodSense.Common.Models;
using FoodSense.Services;

public class ProductsViewModel
{
    private readonly IProductApiService _productApi;

    public List<ProductDto> Products { get; private set; } = [];
    public ProductDto FormModel { get; set; } = new();
    
    public PaginationModel Pagination { get; } = new();

    public int? EditingProductId { get; private set; }
    public int? ProductToDeleteId { get; private set; }
    public ProductDto? ActivePantryProduct { get; private set; }
    public int? EditingPantryItemId { get; private set; }
    public decimal PantryFormQuantity { get; set; } = 1;

    public bool IsLoading { get; private set; }
    public bool IsSaving { get; private set; }
    public bool IsSavingPantry { get; private set; }
    
    public string? SuccessMessage { get; private set; }
    public string? ErrorMessage { get; private set; }

    public ProductsViewModel(IProductApiService productApi)
    {
        _productApi = productApi;
    }

    public async Task LoadProductsAsync(int page)
    {
        IsLoading = true;
        ClearAlerts();

        try
        {
            var response = await _productApi.GetProductsAsync(page, Pagination.PageSize);
            if (response != null)
            {
                Products = response.Data?.ToList() ?? [];
                Pagination.Update(page, response.TotalCount);

                if (ActivePantryProduct != null)
                {
                    ActivePantryProduct = Products.FirstOrDefault(p => p.Id == ActivePantryProduct.Id);
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task SaveProductAsync()
    {
        IsSaving = true;
        ClearAlerts();

        try
        {
            if (EditingProductId is null)
            {
                var response = await _productApi.CreateProductAsync(FormModel);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"Failed to create product: {errorMsg}";
                    return;
                }
                SuccessMessage = "Product created successfully.";
            }
            else
            {
                FormModel.Id = EditingProductId.Value;
                await _productApi.UpdateProductAsync(EditingProductId.Value, FormModel);
                SuccessMessage = "Product updated successfully.";
            }

            ResetForm();
            await LoadProductsAsync(Pagination.CurrentPage);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsSaving = false;
        }
    }

    public async Task HandleBarcodeInputAsync(string? barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode)) return;

        try
        {
            var product = await _productApi.GetProductByBarcodeAsync(barcode);
            if (product != null) StartEditing(product);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public async Task ExecuteDeleteAsync()
    {
        if (!ProductToDeleteId.HasValue) return;

        IsSaving = true;
        ClearAlerts();

        try
        {
            await _productApi.DeleteProductAsync(ProductToDeleteId.Value);
            SuccessMessage = "Product deleted successfully.";
            ProductToDeleteId = null;
            await LoadProductsAsync(Pagination.CurrentPage);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsSaving = false;
        }
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
    }

    public void PromptDelete(int id) => ProductToDeleteId = id;
    public void CancelDelete() => ProductToDeleteId = null;
    public void ClearAlerts() { SuccessMessage = null; ErrorMessage = null; }

    // --- Pantry Management Handlers ---

    public async Task QuickAddPantryItemAsync(int productId)
    {
        try
        {
            var response = await _productApi.QuickAddPantryItemAsync(productId);
            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "Pantry item added.";
                await LoadProductsAsync(Pagination.CurrentPage);
            }
            else
            {
                ErrorMessage = "Failed to add item to pantry.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public async Task SavePantryItemAsync()
    {
        if (ActivePantryProduct == null || PantryFormQuantity <= 0) return;

        IsSavingPantry = true;
        ErrorMessage = null;

        try
        {
            var payload = new PantryItemDto
            {
                Id = EditingPantryItemId ?? 0,
                Quantity = PantryFormQuantity,
                AddedAt = DateTime.UtcNow
            };

            var response = EditingPantryItemId.HasValue
                ? await _productApi.UpdatePantryItemAsync(EditingPantryItemId.Value, payload)
                : await _productApi.CreatePantryItemAsync(ActivePantryProduct.Id, payload);

            if (response.IsSuccessStatusCode)
            {
                ResetPantryForm();
                await LoadProductsAsync(Pagination.CurrentPage);
            }
            else
            {
                ErrorMessage = "Failed to save pantry item.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsSavingPantry = false;
        }
    }

    public async Task DeletePantryItemAsync(int pantryItemId)
    {
        try
        {
            var response = await _productApi.DeletePantryItemAsync(pantryItemId);
            if (response.IsSuccessStatusCode)
            {
                await LoadProductsAsync(Pagination.CurrentPage);
            }
            else
            {
                ErrorMessage = "Failed to delete pantry item.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public void OpenPantryModal(ProductDto product)
    {
        ActivePantryProduct = product;
        ResetPantryForm();
    }

    public void ClosePantryModal()
    {
        ActivePantryProduct = null;
        ResetPantryForm();
    }

    public void StartEditingPantryItem(PantryItemDto item)
    {
        EditingPantryItemId = item.Id;
        PantryFormQuantity = item.Quantity;
    }

    public void ResetPantryForm()
    {
        EditingPantryItemId = null;
        PantryFormQuantity = 1;
    }
}
using FoodSense.Common.Requests;

namespace FoodSense.BlazorFrontend.Services;
public interface IPantryService
{
    Task<HttpResponseMessage> UpdatePantryItemAsync(EditPantryItemRequest request);
    Task<HttpResponseMessage> QuickAddPantryItemAsync(int productId);
    Task<HttpResponseMessage> CreatePantryItemAsync(AddPantryItemRequest request);
    /// <summary>Adds a pantry item, persisting the product first when it is not in the catalog yet.</summary>
    Task<HttpResponseMessage> AddPantryItemWithProductAsync(AddPantryItemWithProductRequest request);
    Task<HttpResponseMessage> DeletePantryItemAsync(int pantryItemId);
}

public class PantryService(HttpClient httpClient) : IPantryService
{
    public Task<HttpResponseMessage> QuickAddPantryItemAsync(int productId)
    {
        var request = new AddPantryItemRequest
        {
            ProductId = productId,
            Quantity = 1,
        };

        return CreatePantryItemAsync(request);
    }

    public async Task<HttpResponseMessage> CreatePantryItemAsync(AddPantryItemRequest request)
    {
        return await httpClient.PostAsJsonAsync($"api/Pantry/CreatePantryItem", request);
    }

    public async Task<HttpResponseMessage> AddPantryItemWithProductAsync(AddPantryItemWithProductRequest request)
    {
        return await httpClient.PostAsJsonAsync($"api/Pantry/CreatePantryItemWithProduct", request);
    }

    public async Task<HttpResponseMessage> UpdatePantryItemAsync(EditPantryItemRequest request)
    {
        return await httpClient.PutAsJsonAsync($"api/Pantry/UpdatePantryItem", request);
    }

    public async Task<HttpResponseMessage> DeletePantryItemAsync(int pantryItemId)
    {
        return await httpClient.DeleteAsync($"api/Pantry/DeletePantryItem/{pantryItemId}");
    }
}
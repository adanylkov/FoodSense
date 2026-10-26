using FoodSense.API.Data.Models;
using FoodSense.Common.Models;

namespace FoodSense.BlazorFrontend.Services;

public interface ICalorieTrackerApiService
{
    Task<HttpResponseMessage> LogAndConsumeAsync(int productId, decimal quantity, int? pantryItemId = null, MealType mealType = MealType.Lunch);
    Task<UserNutritionalGoals?> GetUserGoalsAsync();
    Task<List<FoodLogEntry>> GetFoodLogEntriesAsync(DateTime date);
}

public class CalorieTrackerApiService : ICalorieTrackerApiService
{
    private readonly HttpClient _http;

    public CalorieTrackerApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<HttpResponseMessage> LogAndConsumeAsync(int productId, decimal quantity, int? pantryItemId = null, MealType mealType = MealType.Lunch)
    {
        return await _http.PostAsJsonAsync("api/CalorieTracker/LogAndConsumeFromPantry", new LogConsumptionRequest
        {
            ProductId = productId,
            Quantity = quantity,
            PantryItemId = pantryItemId,
            MealType = mealType
        });
    }

    public async Task<List<FoodLogEntry>> GetFoodLogEntriesAsync(DateTime date)
    {
        return await _http.GetFromJsonAsync<List<FoodLogEntry>>($"api/CalorieTracker/GetEntries?date={date:yyyy-MM-dd}") ?? [];
    }

    public async Task<UserNutritionalGoals?> GetUserGoalsAsync()
    {
        return await _http.GetFromJsonAsync<UserNutritionalGoals>("api/CalorieTracker/GetGoals");
    }
}

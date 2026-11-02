using FoodSense.Common.Models;
using FoodSense.Common.Requests;

namespace FoodSense.BlazorFrontend.Services;

public interface ICalorieTrackerApiService
{
    Task<HttpResponseMessage> LogConsumptionAsync(AddFoodLogRequest request);
    Task<HttpResponseMessage> UpdateConsumptionAsync(EditFoodLogRequest request);
    Task<UserNutritionalGoalsDto?> GetUserGoalsAsync();
    Task<List<FoodLogEntryDto>> GetFoodLogEntriesAsync(DateTime date);
    Task<HttpResponseMessage> DeleteFoodLogEntryAsync(int id);
}

public class CalorieTrackerApiService : ICalorieTrackerApiService
{
    private readonly HttpClient _http;

    public CalorieTrackerApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<HttpResponseMessage> LogConsumptionAsync(AddFoodLogRequest request)
    {
        return await _http.PostAsJsonAsync("api/CalorieTracker/LogConsumption", request);
    }

    public async Task<HttpResponseMessage> UpdateConsumptionAsync(EditFoodLogRequest request)
    {
        return await _http.PutAsJsonAsync("api/CalorieTracker/UpdateConsumption", request);
    }

    public async Task<List<FoodLogEntryDto>> GetFoodLogEntriesAsync(DateTime date)
    {
        return await _http.GetFromJsonAsync<List<FoodLogEntryDto>>($"api/CalorieTracker/GetEntries?date={date:yyyy-MM-dd}") ?? [];
    }

    public async Task<UserNutritionalGoalsDto?> GetUserGoalsAsync()
    {
        return await _http.GetFromJsonAsync<UserNutritionalGoalsDto>("api/CalorieTracker/GetGoals");
    }

    public Task<HttpResponseMessage> DeleteFoodLogEntryAsync(int id)
    {
        return _http.DeleteAsync($"api/CalorieTracker/DeleteConsumption/{id}");
    }
}

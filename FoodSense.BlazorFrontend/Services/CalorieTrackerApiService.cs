using FoodSense.API.Data.Models;
using FoodSense.Common.Models;
using FoodSense.Common.Requests;

namespace FoodSense.BlazorFrontend.Services;

public interface ICalorieTrackerApiService
{
    Task<HttpResponseMessage> LogAndConsumeAsync(AddFoodLogRequest request);
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

    public async Task<HttpResponseMessage> LogAndConsumeAsync(AddFoodLogRequest request)
    {
        return await _http.PostAsJsonAsync("api/CalorieTracker/LogAndConsumeFromPantry", request);
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

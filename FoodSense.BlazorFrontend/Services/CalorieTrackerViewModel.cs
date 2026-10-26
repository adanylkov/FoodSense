namespace FoodSense.BlazorFrontend.ViewModels;

using FoodSense.Common.Models;
using FoodSense.BlazorFrontend.Services;
using FoodSense.ViewModels;
using FoodSense.Services;
using FoodSense.API.Data.Models;
using Microsoft.AspNetCore.Components;

public class CalorieTrackerViewModel : ViewModelBase, IDisposable
{
    private readonly IProductApiService _productApi;
    private readonly ICalorieTrackerApiService _calorieTrackerApi;
    
    public DateTime SelectedDate { get; private set; } = DateTime.Today;
    public List<FoodLogEntry> LogEntries { get; private set; } = [];
    public UserNutritionalGoals Goals { get; private set; } = new();
    public bool IsLoading { get; private set; }

    // Obliczane sumy dzienne
    public double TotalCalories => LogEntries.Sum(e => e.Calories);
    public double TotalProtein => LogEntries.Sum(e => e.Protein);
    public double TotalFat => LogEntries.Sum(e => e.Fat);
    public double TotalCarbs => LogEntries.Sum(e => e.Carbs);

    public CalorieTrackerViewModel(IProductApiService productApi, AlertService alerts, ICalorieTrackerApiService calorieTrackerApi, AddProductModalViewModel addProductModalViewModel) : base(alerts)
    {
        _productApi = productApi;
        _calorieTrackerApi = calorieTrackerApi;
        addProductModalViewModel.ProductAdded += async () => await LoadAsync();
    }

    public async Task LoadAsync(DateTime? date = null)
    {
        if (date.HasValue) SelectedDate = date.Value;
        Alerts.Clear();

        await RunAsync(async () =>
        {
            var entriesTask = _calorieTrackerApi.GetFoodLogEntriesAsync(SelectedDate);
            var goalsTask = _calorieTrackerApi.GetUserGoalsAsync();

            await Task.WhenAll(entriesTask, goalsTask);

            LogEntries = await entriesTask;
            Goals = await goalsTask ?? new();
        }, busy => IsLoading = busy);
    }

    public async Task ChangeDateAsync(ChangeEventArgs e)
    {
        if (DateTime.TryParse(e.Value?.ToString(), out var newDate))
        {
            await LoadAsync(newDate);
        }
    }

    public void Dispose()
    {
        // Sprzątanie subskrypcji jeśli występują
    }
}
namespace FoodSense.BlazorFrontend.ViewModels;

using FoodSense.Common.Models;
using FoodSense.BlazorFrontend.Services;
using FoodSense.ViewModels;
using FoodSense.Services;
using Microsoft.AspNetCore.Components;

public class CalorieTrackerListViewModel : ViewModelBase, IDisposable
{
    private readonly ICalorieTrackerApiService _calorieTrackerApi;
    
    public DateTime SelectedDate { get; private set; } = DateTime.Today;
    public List<FoodLogEntryDto> LogEntries { get; private set; } = [];
    public UserNutritionalGoalsDto Goals { get; private set; } = new();
    public bool IsLoading { get; private set; }
    public TotalNutrients TotalNutrients { get; private set; }

    public CalorieTrackerListViewModel(AlertService alerts, ICalorieTrackerApiService calorieTrackerApi, AddCalorieTrackerViewModel addProductModalViewModel, EditCalorieTrackerViewModel editProductModalViewModel) : base(alerts)
    {
        _calorieTrackerApi = calorieTrackerApi;
        addProductModalViewModel.ProductAdded += async () => await LoadAsync();
        editProductModalViewModel.EntryUpdated += async () => await LoadAsync();
        TotalNutrients = new TotalNutrients();
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
            TotalNutrients = CalculateTotalNutrients();

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

    private TotalNutrients CalculateTotalNutrients()
    {
        var totalCalories = LogEntries.Sum(entry => entry.Nutritions.Calories);
        var totalProtein = LogEntries.Sum(entry => entry.Nutritions.Protein);
        var totalFat = LogEntries.Sum(entry => entry.Nutritions.Fat);
        var totalCarbs = LogEntries.Sum(entry => entry.Nutritions.Carbs);

        return new TotalNutrients
        {
            Calories = totalCalories,
            Protein = totalProtein,
            Fat = totalFat,
            Carbs = totalCarbs
        };
    }

    public void Dispose()
    {
        // Sprzątanie subskrypcji jeśli występują
    }
}

public record TotalNutrients
{
    public double Calories { get; init; }
    public double Protein { get; init; }
    public double Fat { get; init; }
    public double Carbs { get; init; }
}
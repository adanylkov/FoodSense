namespace FoodSense.BlazorFrontend.ViewModels;

using FoodSense.Common.Models;
using FoodSense.ViewModels;
using FoodSense.BlazorFrontend.Services;
using FoodSense.Common.Requests;
using Microsoft.AspNetCore.Components;

public class EditCalorieTrackerViewModel(AlertService alerts, ICalorieTrackerApiService calorieTrackerApi) : ViewModelBase(alerts)
{
    public bool IsOpen { get; private set; }
    public FoodLogEntryDto? CurrentEntry { get; private set; }
    public NutritionSnapshotDto OriginalNutritions { get; private set; } = new NutritionSnapshotDto(0, 0, 0, 0);
    public NutritionSnapshotDto CurrentNutritions { get; private set; } = new NutritionSnapshotDto(0, 0, 0, 0);
    public decimal Quantity { get; private set; } = 0;
    public MealType MealType { get; private set; }

    public event Func<Task>? EntryUpdated;

    public void Open(FoodLogEntryDto entry)
    {
        IsOpen = true;
        CurrentEntry = entry;
        OriginalNutritions = entry.Nutritions;
        CurrentNutritions = entry.Nutritions with
        {
            Calories = Math.Round(entry.Nutritions.Calories, 1),
            Protein = Math.Round(entry.Nutritions.Protein, 1),
            Fat = Math.Round(entry.Nutritions.Fat, 1),
            Carbs = Math.Round(entry.Nutritions.Carbs, 1)
        };
        Quantity = entry.ConsumedQuantity;
        MealType = entry.MealType;
        NotifyStateChanged();
    }

    public void Close()
    {
        IsOpen = false;
        NotifyStateChanged();
    }

    public void SetQuantity(decimal quantity) => AdjustFromQuantity(quantity);
    public void SetCalories(double calories) => AdjustFromMacro(calories / CurrentNutritions.Calories);
    public void SetProtein(double protein) => AdjustFromMacro(protein / CurrentNutritions.Protein);
    public void SetFat(double fat) => AdjustFromMacro(fat / CurrentNutritions.Fat);
    public void SetCarbs(double carbs) => AdjustFromMacro(carbs / CurrentNutritions.Carbs);

    public void SetMealType(ChangeEventArgs args)
    {
        if (Enum.TryParse<MealType>(args.Value?.ToString(), out var mealType))
        {
            MealType = mealType;
            NotifyStateChanged();
        }
    }

    public async Task SaveAsync()
    {
        if (CurrentEntry is null || Quantity <= 0)
        {
            Alerts.Error = "Ilość musi być większa od zera.";
            NotifyStateChanged();
            return;
        }

        await RunAsync(async () =>
        {
            var response = await calorieTrackerApi.UpdateConsumptionAsync(new EditFoodLogRequest
            {
                FoodLogEntryId = CurrentEntry.Id,
                Quantity = Quantity,
                MealType = MealType
            });

            if (!response.IsSuccessStatusCode)
            {
                Alerts.Error = "Nie udało się zapisać zmian.";
                return;
            }

            Alerts.Success = "Zapisano zmiany.";
            Close();
            if (EntryUpdated is not null)
            {
                await EntryUpdated.Invoke();
            }
        });
    }

    public async Task RemoveItem()
    {
        if (CurrentEntry is null)
        {
            return;
        }

        await RunAsync(async () =>
        {
            var response = await calorieTrackerApi.DeleteFoodLogEntryAsync(CurrentEntry.Id);
            if (!response.IsSuccessStatusCode)
            {
                Alerts.Error = "Nie udało się usunąć wpisu.";
                return;
            }

            Alerts.Success = "Usunięto wpis.";
            Close();
            if (EntryUpdated is not null)
            {
                await EntryUpdated.Invoke();
            }
        });
    }

    private void AdjustFromQuantity(decimal quantity)
    {
        if (quantity <= 0 || Quantity <= 0)
        {
            Quantity = quantity;
            NotifyStateChanged();
            return;
        }

        var ratio = (double)(quantity / Quantity);
        Quantity = quantity;
        CurrentNutritions = new NutritionSnapshotDto(
            Math.Round(CurrentNutritions.Calories * ratio, 1),
            Math.Round(CurrentNutritions.Protein * ratio, 1),
            Math.Round(CurrentNutritions.Fat * ratio, 1),
            Math.Round(CurrentNutritions.Carbs * ratio, 1)
        );
        NotifyStateChanged();
    }

    private void AdjustFromMacro(double ratio)
    {
        if (ratio <= 0)
        {
            return;
        }

        CurrentNutritions = CurrentNutritions with
        {
            Calories = Math.Round(CurrentNutritions.Calories * ratio, 1),
            Protein = Math.Round(CurrentNutritions.Protein * ratio, 1),
            Fat = Math.Round(CurrentNutritions.Fat * ratio, 1),
            Carbs = Math.Round(CurrentNutritions.Carbs * ratio, 1)
        };
        Quantity = Math.Round(Quantity * (decimal)ratio, 2);

        NotifyStateChanged();
    }
}
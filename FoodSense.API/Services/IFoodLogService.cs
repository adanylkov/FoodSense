using FoodSense.API.Data.Models;
using FoodSense.Common.Models;
using FoodSense.Common.Requests;


namespace FoodSense.API.Services;

public interface IFoodLogService
{
    Task<Result> LogConsumptionAndReducePantryAsync(AddFoodLogRequest request);
    Task<UserNutritionalGoals?> GetUserGoalsAsync();
    Task<IEnumerable<FoodLogEntry>> GetEntriesByDateAsync(DateTime date);
    Task<Result> EditConsumptionAsync(EditFoodLogRequest request);
}

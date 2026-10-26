using FoodSense.API.Data.Models;
using FoodSense.Common.Models;
using Microsoft.Extensions.Logging.Abstractions;


namespace FoodSense.API.Services;

public interface IFoodLogService
{
    Task<Result> LogConsumptionAndReducePantryAsync(LogConsumptionRequest request);
    Task<UserNutritionalGoals?> GetUserGoalsAsync();
    Task<IEnumerable<FoodLogEntry>> GetEntriesByDateAsync(DateTime date);
}

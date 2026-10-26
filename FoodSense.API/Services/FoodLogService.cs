using FoodSense.Common.Models;
using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodSense.API.Services;

public class FoodLogService(FoodSenseDbContext context) : IFoodLogService
{
    public async Task<IEnumerable<FoodLogEntry>> GetEntriesByDateAsync(DateTime date)
    {
        return await context.FoodLogEntries
            .AsNoTracking()
            .Include(e => e.Product)
            .Where(e => e.Date.Date == date.Date)
            .Select(e => new FoodLogEntry
            {
                Id = e.Id,
                ProductId = e.ProductId,
                Product = e.Product,
                MealType = e.MealType,
                ConsumedQuantity = e.ConsumedQuantity,
                Calories = e.Calories,
                Protein = e.Protein,
                Fat = e.Fat,
                Carbs = e.Carbs,
                Date = e.Date
            })
            .ToListAsync();
    }

    public async Task<UserNutritionalGoals?> GetUserGoalsAsync()
    {
        return await context.UserNutritionalGoals
            .AsNoTracking()
            .FirstOrDefaultAsync() ?? new UserNutritionalGoals();
    }

    public async Task<Result> LogConsumptionAndReducePantryAsync(LogConsumptionRequest request)
    {
        if (request.ProductId <= 0)
        {
            return Result.Failure("A valid product is required.");
        }

        if (request.Quantity <= 0)
        {
            return Result.Failure("Quantity must be greater than zero.");
        }

        // Pobieramy strategię wykonawczą zgodną z konfiguracją SQL Server (retrying strategy)
        var strategy = context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                // 1. Pobierz produkt wraz z jego wartościami odżywczymi
                var product = await context.Products
                    .Include(p => p.Nutrients)
                    .FirstOrDefaultAsync(p => p.Id == request.ProductId);

                if (product == null)
                {
                    return Result.Failure("Product not found.");
                }

                if (product.Nutrients == null)
                {
                    return Result.Failure("Product nutritional information is missing.");
                }

                // 2. Przelicz makroskładniki proporcjonalnie do zjedzonej ilości
                var ratio = (double)(request.Quantity / 100);
                
                var calories = product.Nutrients.EnergyKcal * ratio;
                var protein = product.Nutrients.Proteins * ratio;
                var fat = product.Nutrients.Fat * ratio;
                var carbs = product.Nutrients.Carbohydrates * ratio;

                // 3. Utwórz wpis w dzienniku kalorii
                var logEntry = new FoodLogEntry
                {
                    ProductId = request.ProductId,
                    ConsumedQuantity = request.Quantity,
                    Date = DateTime.UtcNow.Date,
                    MealType = request.MealType,
                    Calories = calories,
                    Protein = protein,
                    Fat = fat,
                    Carbs = carbs
                };

                await context.FoodLogEntries.AddAsync(logEntry);

                // 4. Integracja ze spiżarnią – pomniejsz stan magazynowy
                PantryItem? pantryItem = null;

                if (request.PantryItemId.HasValue)
                {
                    pantryItem = await context.PantryItems.FindAsync(request.PantryItemId.Value);
                }
                else
                {
                    pantryItem = await context.PantryItems
                        .Where(pi => pi.ProductId == request.ProductId && pi.Quantity > 0)
                        .OrderBy(pi => pi.Id)
                        .FirstOrDefaultAsync();
                }

                if (pantryItem != null)
                {
                    pantryItem.Quantity = Math.Max(0, pantryItem.Quantity - request.Quantity);
                    context.PantryItems.Update(pantryItem);
                }

                // 5. Zapisz zmiany i zatwierdź transakcję
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure($"An error occurred while logging consumption: {ex.Message}");
            }
        });
    }
}
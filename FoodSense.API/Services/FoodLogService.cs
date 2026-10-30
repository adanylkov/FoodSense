using FoodSense.Common.Models;
using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using FoodSense.Common.Requests;

namespace FoodSense.API.Services;

public class FoodLogService(FoodSenseDbContext context, ILogger<FoodLogService> logger) : IFoodLogService
{
    public async Task<Result> EditConsumptionAsync(EditFoodLogRequest request)
    {
        var entry = context.FoodLogEntries.FirstOrDefault(e => e.Id == request.FoodLogEntryId);
        if (entry is null)
        {
            logger.LogWarning("Food log entry with ID {FoodLogEntryId} not found.", request.FoodLogEntryId);
            return Result.Failure("Food log entry not found.");
        }

        var product = context.Products
            .Include(p => p.Nutrients)
            .FirstOrDefault(p => p.Id == entry.ProductId);

        if (product is null || product.Nutrients is null)
        {
            logger.LogWarning("Product with ID {ProductId} or its nutritional information not found.", entry.ProductId);
            return Result.Failure("Product or its nutritional information not found.");
        }


        var newEntry = FoodLogEntryFactory.Create(product, request);

        newEntry.Id = entry.Id;
        newEntry.UserId = entry.UserId;
        newEntry.Date = entry.Date;

        context.FoodLogEntries.Update(entry);
        await context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<IEnumerable<FoodLogEntry>> GetEntriesByDateAsync(DateTime date)
    {
        return await context.FoodLogEntries
            .AsNoTracking()
            .Include(e => e.Product)
            .Where(e => e.Date.Date == date.Date)
            .ToListAsync();
    }

    public async Task<UserNutritionalGoals?> GetUserGoalsAsync()
    {
        return await context.UserNutritionalGoals
            .AsNoTracking()
            .FirstOrDefaultAsync() ?? new UserNutritionalGoals();
    }

    public async Task<Result> LogConsumptionAndReducePantryAsync(AddFoodLogRequest request)
    {
        if (request.ProductId <= 0)
        {
            logger.LogWarning("Invalid product ID: {ProductId}", request.ProductId);
            return Result.Failure("A valid product is required.");
        }

        if (request.Quantity <= 0)
        {
            logger.LogWarning("Invalid quantity: {Quantity}", request.Quantity);
            return Result.Failure("Quantity must be greater than zero.");
        }

        var strategy = context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var product = await context.Products
                    .Include(p => p.Nutrients)
                    .FirstOrDefaultAsync(p => p.Id == request.ProductId);

                if (product is null)
                {
                    logger.LogWarning("Product with ID {ProductId} not found.", request.ProductId);
                    return Result.Failure("Product not found.");
                }

                if (product.Nutrients is null)
                {
                    logger.LogWarning("Nutritional information for product ID {ProductId} is missing.", request.ProductId);
                    return Result.Failure("Product nutritional information is missing.");
                }

                var logEntry = FoodLogEntryFactory.Create(product, request);

                await context.FoodLogEntries.AddAsync(logEntry);

                // 5. Zapisz zmiany i zatwierdź transakcję
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while logging consumption and reducing pantry.");
                await transaction.RollbackAsync();
                return Result.Failure($"An error occurred while logging consumption: {ex.Message}");
            }
        });
    }
}
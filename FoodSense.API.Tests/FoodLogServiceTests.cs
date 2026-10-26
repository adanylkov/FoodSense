using FluentAssertions;
using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using FoodSense.API.Services;
using FoodSense.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FoodSense.API.Tests;

public class FoodLogServiceTests
{
    [Fact]
    public async Task LogConsumptionAndReducePantryAsync_ShouldRejectInvalidProductId()
    {
        await using var context = CreateContext();
        var service = new FoodLogService(context);

        var result = await service.LogConsumptionAndReducePantryAsync(new LogConsumptionRequest
        {
            ProductId = 0,
            Quantity = 100
        });

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("A valid product is required.");
        (await context.FoodLogEntries.CountAsync()).Should().Be(0);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task LogConsumptionAndReducePantryAsync_ShouldRejectNonPositiveQuantity(decimal quantity)
    {
        await using var context = CreateContext();
        var service = new FoodLogService(context);

        var result = await service.LogConsumptionAndReducePantryAsync(new LogConsumptionRequest
        {
            ProductId = 1,
            Quantity = quantity
        });

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Quantity must be greater than zero.");
        (await context.FoodLogEntries.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task LogConsumptionAndReducePantryAsync_ShouldLogNutrientsAndReduceMatchingPantryItem()
    {
        await using var context = CreateContext();
        var product = new Product
        {
            ProductName = "Test product",
            Nutrients = new Nutrients
            {
                EnergyKcal = 250,
                Proteins = 10,
                Fat = 8,
                Carbohydrates = 30
            }
        };
        product.PantryItems.Add(new PantryItem { Quantity = 500 });
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var service = new FoodLogService(context);
        var result = await service.LogConsumptionAndReducePantryAsync(new LogConsumptionRequest
        {
            ProductId = product.Id,
            Quantity = 200,
            MealType = MealType.Dinner
        });

        result.IsSuccess.Should().BeTrue();

        var logEntry = await context.FoodLogEntries.SingleAsync();
        logEntry.ProductId.Should().Be(product.Id);
        logEntry.ConsumedQuantity.Should().Be(200);
        logEntry.MealType.Should().Be(MealType.Dinner);
        logEntry.Calories.Should().Be(500);
        logEntry.Protein.Should().Be(20);
        logEntry.Fat.Should().Be(16);
        logEntry.Carbs.Should().Be(60);

        var pantryItem = await context.PantryItems.SingleAsync();
        pantryItem.Quantity.Should().Be(300);
    }

    private static FoodSenseDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<FoodSenseDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new FoodSenseDbContext(options);
    }
}

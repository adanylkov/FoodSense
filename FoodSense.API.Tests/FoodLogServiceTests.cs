using FluentAssertions;
using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using FoodSense.API.Data.ValueObjects;
using FoodSense.API.Services;
using FoodSense.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace FoodSense.API.Tests;

public class FoodLogServiceTests
{
    [Fact]
    public async Task LogConsumptionAsync_ShouldRejectInvalidProductId()
    {
        await using var context = CreateContext();
        var logger = new Mock<ILogger<FoodLogService>>().Object;
        var service = new FoodLogService(context , logger);

        var result = await service.LogConsumptionAsync(new Common.Requests.AddFoodLogRequest
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
    public async Task LogConsumptionAsync_ShouldRejectNonPositiveQuantity(decimal quantity)
    {
        await using var context = CreateContext();
        var logger = new Mock<ILogger<FoodLogService>>().Object;
        var service = new FoodLogService(context , logger);

        var result = await service.LogConsumptionAsync(new Common.Requests.AddFoodLogRequest
        {
            ProductId = 1,
            Quantity = quantity
        });

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Quantity must be greater than zero.");
        (await context.FoodLogEntries.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task LogConsumptionAsync_ShouldLogNutrients()
    {
        await using var context = CreateContext();
        var product = new Product
        {
            Name = "Test product",
            Nutrients = new Nutrients
            {
                Kcal = 250,
                Proteins = 10,
                Fat = 8,
                Carbohydrates = 30
            }
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var logger = new Mock<ILogger<FoodLogService>>().Object;
        var service = new FoodLogService(context , logger);
        var result = await service.LogConsumptionAsync(new Common.Requests.AddFoodLogRequest
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
        logEntry.Nutritions.Calories.Should().Be(500);
        logEntry.Nutritions.Protein.Should().Be(20);
        logEntry.Nutritions.Fat.Should().Be(16);
        logEntry.Nutritions.Carbs.Should().Be(60);
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

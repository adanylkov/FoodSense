using FluentAssertions;
using FoodSense.API.Controllers;
using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using FoodSense.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FoodSense.API.Tests;

public class CategoryControllerTests
{
    [Fact]
    public async Task GetCategories_ShouldOrderRootsAndChildren_AtAnyDepth()
    {
        await using var context = CreateContext();
        var vegetables = new Category { Name = "Vegetables" };
        var fruits = new Category { Name = "Fruits" };
        var zucchini = new Category { Name = "Zucchini", Parent = vegetables };
        var carrots = new Category { Name = "Carrots", Parent = vegetables };
        var yellowCarrots = new Category { Name = "Yellow carrots", Parent = carrots };

        context.Categories.AddRange(vegetables, fruits, zucchini, carrots, yellowCarrots);
        await context.SaveChangesAsync();

        var controller = new CategoryController(context, new MemoryCache(new MemoryCacheOptions()));

        var result = await controller.GetCategories(CancellationToken.None);

        var categories = result.Result.Should().BeOfType<OkObjectResult>().Subject
            .Value.Should().BeAssignableTo<IEnumerable<CategoryDto>>().Subject
            .ToList();

        categories.Select(category => category.Name)
            .Should().Equal("Fruits", "Vegetables");

        var vegetablesDto = categories.Single(category => category.Name == "Vegetables");
        vegetablesDto.Children.Select(category => category.Name)
            .Should().Equal("Carrots", "Zucchini");
        vegetablesDto.Children.Single(category => category.Name == "Carrots")
            .Children.Select(category => category.Name)
            .Should().Equal("Yellow carrots");
    }

    [Fact]
    public async Task GetCategories_ShouldUseCachedTree()
    {
        await using var context = CreateContext();
        context.Categories.Add(new Category { Name = "Fruits" });
        await context.SaveChangesAsync();

        var cache = new MemoryCache(new MemoryCacheOptions());
        var controller = new CategoryController(context, cache);

        var firstResult = await controller.GetCategories(CancellationToken.None);
        context.Categories.Add(new Category { Name = "Vegetables" });
        await context.SaveChangesAsync();

        var secondResult = await controller.GetCategories(CancellationToken.None);

        var firstCategories = firstResult.Result.Should().BeOfType<OkObjectResult>().Subject
            .Value.Should().BeAssignableTo<IEnumerable<CategoryDto>>().Subject;
        var secondCategories = secondResult.Result.Should().BeOfType<OkObjectResult>().Subject
            .Value.Should().BeAssignableTo<IEnumerable<CategoryDto>>().Subject;

        secondCategories.Should().BeSameAs(firstCategories);
        secondCategories.Select(category => category.Name)
            .Should().Equal("Fruits");
    }

    private static FoodSenseDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<FoodSenseDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FoodSenseDbContext(options);
    }
}

using FluentAssertions;
using FoodSense.API.Controllers;
using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodSense.API.Tests;

public class FoodControllerTests
{
    [Fact]
    public async Task GetProducts_ShouldReturnPagedResults()
    {
        await using var context = CreateContext();
        for (var i = 1; i <= 120; i++)
        {
            context.Products.Add(new Product { Barcode = i.ToString() });
        }
        await context.SaveChangesAsync();

        var controller = new FoodController(context, null!, null!);

        var result = await controller.GetProducts(page: 2, pageSize: 25);
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var products = okResult.Value.Should().BeAssignableTo<IEnumerable<Product>>().Subject.ToList();

        products.Should().HaveCount(25);
        products.First().Barcode.Should().Be("26");
        products.Last().Barcode.Should().Be("50");
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    [InlineData(1, 201)]
    public async Task GetProducts_ShouldRejectInvalidPagination(int page, int pageSize)
    {
        await using var context = CreateContext();
        var controller = new FoodController(context, null!, null!);

        var result = await controller.GetProducts(page, pageSize);

        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task CreateProduct_ShouldPersistAndReturnCreated()
    {
        await using var context = CreateContext();
        var controller = new FoodController(context, null!, null!);

        var createdProduct = new Product { Barcode = "5901234567890" };
        var result = await controller.CreateProduct(createdProduct);

        var createdAt = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAt.ActionName.Should().Be(nameof(FoodController.GetProductById));
        context.Products.Should().ContainSingle(p => p.Barcode == "5901234567890");
    }

    [Fact]
    public async Task CreateProduct_ShouldRejectDuplicateBarcode()
    {
        await using var context = CreateContext();
        context.Products.Add(new Product { Barcode = "111" });
        await context.SaveChangesAsync();

        var controller = new FoodController(context, null!, null!);
        var result = await controller.CreateProduct(new Product { Barcode = "111" });

        result.Result.Should().BeOfType<ConflictObjectResult>();
    }

    [Fact]
    public async Task UpdateProduct_ShouldUpdateExistingEntity()
    {
        await using var context = CreateContext();
        var existing = new Product { Barcode = "111" };
        context.Products.Add(existing);
        await context.SaveChangesAsync();

        var controller = new FoodController(context, null!, null!);
        var updated = new Product { Barcode = "222" };

        var result = await controller.UpdateProduct(existing.Id, updated);

        result.Result.Should().BeOfType<OkObjectResult>();
        var dbProduct = await context.Products.FindAsync(existing.Id);
        dbProduct.Should().NotBeNull();
        dbProduct!.Barcode.Should().Be("222");
    }

    [Fact]
    public async Task DeleteProduct_ShouldRemoveEntity()
    {
        await using var context = CreateContext();
        var existing = new Product { Barcode = "111" };
        context.Products.Add(existing);
        await context.SaveChangesAsync();

        var controller = new FoodController(context, null!, null!);
        var result = await controller.DeleteProduct(existing.Id);

        result.Should().BeOfType<NoContentResult>();
        (await context.Products.FindAsync(existing.Id)).Should().BeNull();
    }

    private static FoodSenseDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<FoodSenseDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FoodSenseDbContext(options);
    }
}

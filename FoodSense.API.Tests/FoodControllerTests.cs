using FluentAssertions;
using FoodSense.API.Controllers;
using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using FoodSense.API.Repositories;
using FoodSense.API.Services;
using FoodSense.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Moq;

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


        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, null!);
        var controller = new FoodController(productService);

        var result = await controller.GetProducts(page: 2, pageSize: 25);
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeAssignableTo<ApiResponse<IEnumerable<ProductDto>>>().Subject;
        var products = response.Data;

        products.Should().HaveCount(25);
        products.First().Barcode.Should().Be("26");
        products.Last().Barcode.Should().Be("50");
    }

    [Fact]
    public async Task GetProducts_WithPantryFilter_ShouldReturnOnlyProductsInStock()
    {
        await using var context = CreateContext();
        context.Products.AddRange(
            new Product
            {
                Barcode = "1",
                PantryItems = new List<PantryItem> { new() { Quantity = 2 } }
            },
            new Product
            {
                Barcode = "2",
                PantryItems = new List<PantryItem> { new() { Quantity = 0 } }
            },
            new Product { Barcode = "3" });
        await context.SaveChangesAsync();

        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, null!, null!);
        var controller = new FoodController(productService);

        var result = await controller.GetProducts(page: 1, pageSize: 25, hasPantryItems: true);
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeAssignableTo<ApiResponse<IEnumerable<ProductDto>>>().Subject;

        response.Data.Should().ContainSingle().Which.Barcode.Should().Be("1");
        response.TotalCount.Should().Be(1);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    [InlineData(1, 201)]
    public async Task GetProducts_ShouldRejectInvalidPagination(int page, int pageSize)
    {
        await using var context = CreateContext();
        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, null!);
        var controller = new FoodController(productService);

        var result = await controller.GetProducts(page, pageSize);

        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task CreateProduct_ShouldPersistAndReturnCreated()
    {
        await using var context = CreateContext();
        var productRepository = new ProductRepository(context);
        var opfClientMock = new Mock<IOpenFoodFactsClient>();
        opfClientMock.Setup(client => client.GetProductByBarcodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((OpenFoodFactProduct?)null);


        var productService = new ProductService(productRepository, opfClientMock.Object);
        var controller = new FoodController(productService);

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

        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, null!);
        var controller = new FoodController(productService);
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

        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, null!);
        var controller = new FoodController(productService);
        var updated = new Product { Barcode = "222" };

        var result = await controller.UpdateProduct(existing.Id, updated);

        result.Result.Should().BeOfType<OkObjectResult>();
        var dbProduct = await context.Products.FindAsync(existing.Id);
        dbProduct.Should().NotBeNull();
        dbProduct!.Barcode.Should().Be("222");
    }

    [Fact]
    public async Task UpdateProduct_ShouldPersistOwnedNutrientsChanges()
    {
        await using var context = CreateContext();
        var existing = new Product
        {
            Barcode = "111",
            Nutrients = new Nutrients { EnergyKcal = 100, Fat = 1 }
        };
        context.Products.Add(existing);
        await context.SaveChangesAsync();

        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, null!);
        var controller = new FoodController(productService);
        var updated = new Product
        {
            Barcode = "222",
            Nutrients = new Nutrients { EnergyKcal = 250, Fat = 5, SaturatedFat = 2, Carbohydrates = 30, Sugars = 10, Proteins = 12, Salt = 1.5 }
        };

        var result = await controller.UpdateProduct(existing.Id, updated);

        result.Result.Should().BeOfType<OkObjectResult>();
        var dbProduct = await context.Products.FindAsync(existing.Id);
        dbProduct.Should().NotBeNull();
        dbProduct!.Barcode.Should().Be("222");
        dbProduct.Nutrients.EnergyKcal.Should().Be(250);
        dbProduct.Nutrients.Fat.Should().Be(5);
        dbProduct.Nutrients.Salt.Should().Be(1.5);
    }

    [Fact]
    public async Task DeleteProduct_ShouldRemoveEntity()
    {
        await using var context = CreateContext();
        var existing = new Product { Barcode = "111" };
        context.Products.Add(existing);
        await context.SaveChangesAsync();

        var productRepository = new ProductRepository(context);
        var productService = new ProductService(productRepository, null!);
        var controller = new FoodController(productService);
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

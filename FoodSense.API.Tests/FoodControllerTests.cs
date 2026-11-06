using FluentAssertions;
using FoodSense.API.Controllers;
using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using FoodSense.API.Data.ValueObjects;
using FoodSense.API.Repositories;
using FoodSense.API.Services;
using FoodSense.API.Services.ProductSearch;
using FoodSense.Common.Models;
using Microsoft.AspNetCore.Mvc;
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

        var productService = CreateService(context);
        var controller = new ProductController(productService);

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
        var productInStock = new Product { Barcode = "1" };
        productInStock.PantryItems = new List<PantryItem> { new(productInStock, 2) };
        var productOutOfStock = new Product { Barcode = "2" };
        productOutOfStock.PantryItems = new List<PantryItem> { new(productOutOfStock, 0) };
        context.Products.AddRange(productInStock, productOutOfStock, new Product { Barcode = "3" });
        await context.SaveChangesAsync();

        var productService = CreateService(context);
        var controller = new ProductController(productService);

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
        var productService = CreateService(context);
        var controller = new ProductController(productService);

        var result = await controller.GetProducts(page, pageSize);

        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task CreateProduct_ShouldPersistAndReturnCreated()
    {
        await using var context = CreateContext();
        var productService = CreateService(context);
        var controller = new ProductController(productService);

        var createdProduct = new ProductDto { Barcode = "5901234567890" };
        var result = await controller.CreateProduct(createdProduct);

        var createdAt = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAt.ActionName.Should().Be(nameof(ProductController.GetProductById));
        context.Products.Should().ContainSingle(p => p.Barcode == "5901234567890");
    }

    [Fact]
    public async Task CreateProduct_ShouldPersistFieldsSentAsDto()
    {
        await using var context = CreateContext();
        var productService = CreateService(context);
        var controller = new ProductController(productService);

        var result = await controller.CreateProduct(new ProductDto
        {
            ProductName = "Apple",
            FrontImageUrl = "https://images.openfoodfacts.org/images/products/123/front.jpg",
            Nutrients = new NutrientsDto { EnergyKcal = 52, Proteins = 0.3 }
        });

        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var persisted = context.Products.Single(p => p.Name == "Apple");
        persisted.Nutrients.Kcal.Should().Be(52);
        persisted.Nutrients.Proteins.Should().Be(0.3);
        persisted.FrontImageUrl.Should().Be("https://images.openfoodfacts.org/images/products/123/front.jpg");
    }

    [Fact]
    public async Task CreateProduct_ShouldRejectDuplicateBarcode()
    {
        await using var context = CreateContext();
        context.Products.Add(new Product { Barcode = "111" });
        await context.SaveChangesAsync();

        var productService = CreateService(context);
        var controller = new ProductController(productService);
        var result = await controller.CreateProduct(new ProductDto { Barcode = "111" });

        result.Result.Should().BeOfType<ConflictObjectResult>();
    }

    [Fact]
    public async Task UpdateProduct_ShouldUpdateExistingEntity()
    {
        await using var context = CreateContext();
        var existing = new Product { Barcode = "111" };
        context.Products.Add(existing);
        await context.SaveChangesAsync();

        var productService = CreateService(context);
        var controller = new ProductController(productService);
        var updated = new ProductDto { Id = existing.Id, Barcode = "222" };

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
            Nutrients = new Nutrients { Kcal = 100, Fat = 1 }
        };
        context.Products.Add(existing);
        await context.SaveChangesAsync();

        var productService = CreateService(context);
        var controller = new ProductController(productService);
        var updated = new ProductDto
        {
            Id = existing.Id,
            Barcode = "222",
            Nutrients = new NutrientsDto
            {
                EnergyKcal = 250,
                Fat = 5,
                SaturatedFat = 2,
                Carbohydrates = 30,
                Sugars = 10,
                Proteins = 12,
                Salt = 1.5
            }
        };

        var result = await controller.UpdateProduct(existing.Id, updated);

        result.Result.Should().BeOfType<OkObjectResult>();
        var dbProduct = await context.Products.FindAsync(existing.Id);
        dbProduct.Should().NotBeNull();
        dbProduct!.Barcode.Should().Be("222");
        dbProduct.Nutrients.Kcal.Should().Be(250);
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

        var productService = CreateService(context);
        var controller = new ProductController(productService);
        var result = await controller.DeleteProduct(existing.Id);

        result.Should().BeOfType<NoContentResult>();
        (await context.Products.FindAsync(existing.Id)).Should().BeNull();
    }

    [Fact]
    public async Task SearchProducts_ShouldReturnLocalMatchesFirst()
    {
        // Arrange
        await using var context = CreateContext();
        var local = new Product { Name = "Jogurt naturalny", Barcode = "111", Nutrients = new Nutrients { Kcal = 60 } };
        context.Products.Add(local);
        await context.SaveChangesAsync();

        var openFoodFactsClient = CreateOpenFoodFactsClient(new[]
        {
            new OpenFoodFactProduct
            {
                Code = "222",
                Name = "Jogurt truskawkowy",
                Brand = "Olimp",
                ImageSmallUrl = "https://images.openfoodfacts.org/images/products/222/front.jpg",
                Nutrients = new OpenFoodFactNutrition { Calories = 80 }
            }
        });

        var controller = new ProductController(CreateService(context, openFoodFactsClient.Object));

        // Act
        var okResult = (await controller.SearchProducts("jogurt")).Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeAssignableTo<ApiResponse<IEnumerable<ProductSearchResultDto>>>().Subject;
        var results = response.Data?.ToList() ?? [];

        // Assert
        results.Should().HaveCount(2);
        results[0].Source.Should().Be(ProductSource.Local);
        results[0].ProductId.Should().Be(local.Id);
        results[1].Source.Should().Be(ProductSource.OpenFoodFacts);
        results[1].ProductId.Should().Be(0);
        results[1].IsPersisted.Should().BeFalse();
        results[1].Brand.Should().Be("Olimp");
        results[1].Nutrients.EnergyKcal.Should().Be(80);
    }

    [Fact]
    public async Task SearchProducts_ShouldNotPersistOpenFoodFactsCandidates()
    {
        // Arrange
        await using var context = CreateContext();
        var openFoodFactsClient = CreateOpenFoodFactsClient(new[]
        {
            new OpenFoodFactProduct
            {
                Code = "222",
                Name = "Jogurt truskawkowy",
                Nutrients = new OpenFoodFactNutrition { Calories = 80 }
            }
        });

        var controller = new ProductController(CreateService(context, openFoodFactsClient.Object));

        // Act
        var okResult = (await controller.SearchProducts("jogurt")).Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeAssignableTo<ApiResponse<IEnumerable<ProductSearchResultDto>>>().Subject;

        // Assert
        response.Data.Should().ContainSingle().Which.Source.Should().Be(ProductSource.OpenFoodFacts);
        context.Products.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchProducts_ShouldSkipOpenFoodFactsCandidatesAlreadyInCatalog()
    {
        // Arrange
        await using var context = CreateContext();
        context.Products.Add(new Product { Name = "Jogurt", Barcode = "111", Nutrients = new Nutrients { Kcal = 60 } });
        await context.SaveChangesAsync();

        var openFoodFactsClient = CreateOpenFoodFactsClient(new[]
        {
            new OpenFoodFactProduct { Code = "111", Name = "Jogurt" },
            new OpenFoodFactProduct { Code = "222", Name = "Ser" }
        });

        var controller = new ProductController(CreateService(context, openFoodFactsClient.Object));

        // Act
        var okResult = (await controller.SearchProducts("jogurt")).Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeAssignableTo<ApiResponse<IEnumerable<ProductSearchResultDto>>>().Subject;

        // Assert
        response.Data.Should().HaveCount(2);
        response.Data.Should().ContainSingle(r => r.ProductId > 0).Which.Name.Should().Be("Jogurt");
        response.Data.Should().ContainSingle(r => r.ProductId == 0).Which.Name.Should().Be("Ser");
        context.Products.Should().ContainSingle();
    }

    [Theory]
    [InlineData("", 10)]
    [InlineData("   ", 10)]
    public async Task SearchProducts_ShouldReturnEmptyResultForBlankQuery(string name, int limit)
    {
        // Arrange
        await using var context = CreateContext();
        context.Products.Add(new Product { Name = "Jogurt", Nutrients = new Nutrients { Kcal = 60 } });
        await context.SaveChangesAsync();

        var controller = new ProductController(CreateService(context));

        // Act
        var okResult = (await controller.SearchProducts(name, limit)).Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeAssignableTo<ApiResponse<IEnumerable<ProductSearchResultDto>>>().Subject;

        // Assert
        response.Data.Should().BeEmpty();
        response.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetProductByBarcode_ShouldReturnOpenFoodFactsHitWithoutPersistingIt()
    {
        // Arrange
        await using var context = CreateContext();
        var openFoodFactsClient = new Mock<IOpenFoodFactsClient>();
        openFoodFactsClient
            .Setup(client => client.GetProductByBarcodeAsync("111", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OpenFoodFactProduct
            {
                Code = "111",
                Name = "Mleko",
                Brand = "Mlekpol",
                Nutrients = new OpenFoodFactNutrition { Calories = 44 }
            });

        var service = CreateService(context, openFoodFactsClient.Object);

        // Act
        var product = await service.GetProductByBarcodeAsync("111");

        // Assert
        product.Should().NotBeNull();
        product!.Id.Should().Be(0);
        product.ProductName.Should().Be("Mleko");
        product.Nutrients.EnergyKcal.Should().Be(44);
        context.Products.Should().BeEmpty();
    }

    private static FoodSenseDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<FoodSenseDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FoodSenseDbContext(options);
    }

    private static ProductService CreateService(
        FoodSenseDbContext context,
        IOpenFoodFactsClient? openFoodFactsClient = null)
    {
        return new ProductService(
            new ProductRepository(context),
            new ProductMappingRepository(context),
            [
                new ProductRepositorySearchSource(new ProductRepository(context)),
                new OpenFoodFactsSearchSource(openFoodFactsClient ?? CreateOpenFoodFactsClient().Object)
            ]);
    }

    private static Mock<IOpenFoodFactsClient> CreateOpenFoodFactsClient(
        IEnumerable<OpenFoodFactProduct>? searchResults = null)
    {
        var mock = new Mock<IOpenFoodFactsClient>();
        mock.Setup(client => client.GetProductByBarcodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((OpenFoodFactProduct?)null);
        mock.Setup(client => client.SearchProductsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(searchResults?.ToList() ?? []);

        return mock;
    }
}

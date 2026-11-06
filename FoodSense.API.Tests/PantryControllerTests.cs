using FluentAssertions;
using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using FoodSense.API.Data.ValueObjects;
using FoodSense.API.Repositories;
using FoodSense.API.Services;
using FoodSense.API.Services.ProductSearch;
using FoodSense.Common.Models;
using FoodSense.Common.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace FoodSense.API.Tests;

public class PantryControllerTests
{
    [Fact]
    public async Task AddPantryItemWithProduct_ShouldPersistProductThatIsNotInCatalogYet()
    {
        // Arrange
        await using var context = CreateContext();
        var service = CreatePantryService(context);

        // Act
        var item = await service.AddPantryItemWithProductAsync(new AddPantryItemWithProductRequest
        {
            Product = new ProductDto
            {
                ProductName = "Jogurt",
                Barcode = "111",
                Nutrients = new NutrientsDto { EnergyKcal = 60, Proteins = 3 }
            },
            Quantity = 250
        });

        // Assert
        var persisted = context.Products.Should().ContainSingle().Which;
        persisted.Id.Should().Be(item.ProductId);
        persisted.Name.Should().Be("Jogurt");
        persisted.Nutrients.Kcal.Should().Be(60);
        persisted.Nutrients.Proteins.Should().Be(3);

        var pantryItem = context.PantryItems.Should().ContainSingle().Which;
        pantryItem.ProductId.Should().Be(persisted.Id);
        pantryItem.Quantity.Should().Be(250);
    }

    [Fact]
    public async Task AddPantryItemWithProduct_ShouldReuseProductWithTheSameBarcode()
    {
        // Arrange
        await using var context = CreateContext();
        var existing = new Product { Name = "Jogurt", Barcode = "111", Nutrients = new Nutrients { Kcal = 60 } };
        context.Products.Add(existing);
        await context.SaveChangesAsync();

        var service = CreatePantryService(context);

        // Act
        var item = await service.AddPantryItemWithProductAsync(new AddPantryItemWithProductRequest
        {
            Product = new ProductDto
            {
                ProductName = "Jogurt truskawkowy",
                Barcode = "111",
                Nutrients = new NutrientsDto { EnergyKcal = 80 }
            },
            Quantity = 100
        });

        // Assert
        item.ProductId.Should().Be(existing.Id);
        context.Products.Should().ContainSingle().Which.Name.Should().Be("Jogurt");
    }

    [Fact]
    public async Task AddPantryItemWithProduct_ShouldReuseProductSelectedFromTheCatalog()
    {
        // Arrange
        await using var context = CreateContext();
        var existing = new Product { Name = "Jogurt", Barcode = "111", Nutrients = new Nutrients { Kcal = 60 } };
        context.Products.Add(existing);
        await context.SaveChangesAsync();

        var service = CreatePantryService(context);

        // Act
        var item = await service.AddPantryItemWithProductAsync(new AddPantryItemWithProductRequest
        {
            Product = new ProductDto { Id = existing.Id, ProductName = "Jogurt" },
            Quantity = 100
        });

        // Assert
        item.ProductId.Should().Be(existing.Id);
        context.Products.Should().ContainSingle();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task AddPantryItemWithProduct_ShouldRejectNonPositiveQuantity(decimal quantity)
    {
        // Arrange
        await using var context = CreateContext();
        var service = CreatePantryService(context);
        var request = new AddPantryItemWithProductRequest
        {
            Product = new ProductDto { ProductName = "Jogurt", Nutrients = new NutrientsDto { EnergyKcal = 60 } },
            Quantity = quantity
        };

        // Act + Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.AddPantryItemWithProductAsync(request));
        context.PantryItems.Should().BeEmpty();
    }

    [Fact]
    public async Task AddPantryItemWithProduct_ShouldRejectMissingProduct()
    {
        // Arrange
        await using var context = CreateContext();
        var service = CreatePantryService(context);

        // Act + Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => service.AddPantryItemWithProductAsync(new AddPantryItemWithProductRequest { Quantity = 100 }));
        context.PantryItems.Should().BeEmpty();
    }

    private static FoodSenseDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<FoodSenseDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FoodSenseDbContext(options);
    }

    private static PantryService CreatePantryService(FoodSenseDbContext context)
    {
        var productRepository = new ProductRepository(context);
        var productService = new ProductService(
            productRepository,
            new ProductMappingRepository(context),
            [
                new ProductRepositorySearchSource(productRepository)
            ]);

        return new PantryService(
            new PantryRepository(context),
            productRepository,
            productService,
            new Mock<ILogger<PantryService>>().Object);
    }
}

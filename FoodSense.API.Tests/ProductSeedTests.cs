using FluentAssertions;
using FoodSense.API.Data;
using FoodSense.API.Data.Models;

namespace FoodSense.API.Tests;

/// <summary>Guards the seed of everyday products sold without a barcode (see <see cref="ProductSeed"/>).
/// The seed ships as an EF Core migration, so every rule below is a rule about the shipped data.</summary>
public class ProductSeedTests
{
    /// <summary>Category ids seeded in <c>FoodSenseDbContext.OnModelCreating</c> (Fruits .. Drinks).</summary>
    private static readonly int[] SeededCategoryIds = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];

    [Fact]
    public void SeededProducts_AreSoldWithoutBarcode()
    {
        ProductSeed.Data.Should().NotBeEmpty();
        ProductSeed.Data.Should().AllSatisfy(p => p.Barcode.Should().BeNull(
            "these products are sold without a barcode"));
    }

    [Fact]
    public void SeededProductNames_AreUniqueAndNotEmpty()
    {
        var names = ProductSeed.Data.Select(p => p.Name.ToLowerInvariant()).ToList();

        names.Should().AllSatisfy(n => n.Should().NotBeNullOrWhiteSpace());
        names.Distinct().Should().HaveCount(names.Count, "EF seed data needs unique names");
    }

    [Fact]
    public void SeededProductNames_KeepPolishDiacritics()
    {
        ProductSeed.Data.Should().Contain(p => p.Name.Any(c => "ąćłśżźćĄĘŁŚŃ".Contains(c)),
            "products are matched by their Polish names in the UI");
    }

    [Fact]
    public void SeededProductIds_AreUniqueAndAboveTheRuntimeIdentityRange()
    {
        ProductSeed.Data.Should().AllSatisfy(p => p.Id.Should().BeGreaterThanOrEqualTo(2000));
        ProductSeed.Data.Select(p => p.Id).Distinct().Should().HaveCount(ProductSeed.Data.Length);
    }

    [Fact]
    public void EverySeededProduct_HasExactlyOneNutrientsRow()
    {
        var productIds = ProductSeed.Data.Select(p => p.Id).ToList();
        var nutrientIds = ProductSeed.NutrientsData.Select(n => n.ProductId).ToList();

        nutrientIds.Should().BeEquivalentTo(productIds);
        nutrientIds.Distinct().Should().HaveCount(productIds.Count);
    }

    [Fact]
    public void EverySeededProduct_UsesASeededCategory()
    {
        ProductSeed.Data.Should().AllSatisfy(p => p.CategoryId.Should().BeOneOf(SeededCategoryIds));
    }

    [Fact]
    public void SeededNutrients_HaveEnergyAndNonNegativeMacros()
    {
        ProductSeed.NutrientsData.Should().AllSatisfy(n =>
        {
            n.Kcal.Should().BeGreaterThan(0);
            n.Fat.Should().BeGreaterThanOrEqualTo(0);
            n.SaturatedFat.Should().BeGreaterThanOrEqualTo(0);
            n.Carbohydrates.Should().BeGreaterThanOrEqualTo(0);
            n.Sugars.Should().BeGreaterThanOrEqualTo(0);
            n.Proteins.Should().BeGreaterThanOrEqualTo(0);
            n.Salt.Should().BeGreaterThanOrEqualTo(0);
        });
    }

    [Fact]
    public void SeedCoversEverydayShopping()
    {
        ProductSeed.Data.Should().HaveCountGreaterThanOrEqualTo(60);
        ProductSeed.Data.Select(p => p.CategoryId).Distinct().Should().HaveCountGreaterThanOrEqualTo(4);
    }
}

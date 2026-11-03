namespace FoodSense.Common.Models;

/// <summary>Where a product candidate was found.</summary>
public enum ProductSource
{
    /// <summary>Product is stored in the local catalog.</summary>
    Local,

    /// <summary>Product is known only to OpenFoodFacts and is not persisted locally.</summary>
    OpenFoodFacts
}

/// <summary>
/// A product candidate returned by name search. Candidates with <see cref="ProductId"/> == 0
/// are not stored in the local catalog: they are persisted only when the user explicitly adds
/// the product to the pantry.
/// </summary>
public sealed record ProductSearchResultDto
{
    /// <summary>Local catalog id, 0 when the product is not persisted yet.</summary>
    public int ProductId { get; init; }

    public string? Barcode { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Brand { get; init; } = string.Empty;

    public string PackageSize { get; init; } = string.Empty;

    /// <summary>Thumbnail image URL. Null when the source has no image.</summary>
    public string? ImageUrl { get; init; }

    public NutrientsDto Nutrients { get; init; } = new();

    public ProductSource Source { get; init; }

    /// <summary>True when the candidate already exists in the local catalog.</summary>
    public bool IsPersisted => ProductId > 0;

    /// <summary>Maps a candidate onto the product form. Unpersisted candidates keep <c>Id</c> = 0.</summary>
    public ProductDto ToProductDto() => new()
    {
        Id = ProductId,
        Barcode = Barcode,
        ProductName = Name,
        Brands = Brand,
        ProductQuantity = PackageSize,
        FrontImageUrl = ImageUrl,
        Nutrients = new NutrientsDto
        {
            EnergyKcal = Nutrients.EnergyKcal,
            Fat = Nutrients.Fat,
            SaturatedFat = Nutrients.SaturatedFat,
            Carbohydrates = Nutrients.Carbohydrates,
            Sugars = Nutrients.Sugars,
            Proteins = Nutrients.Proteins,
            Salt = Nutrients.Salt
        }
    };
}

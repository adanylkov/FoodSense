using FoodSense.API.Data.Models;
using FoodSense.Common.Models;

namespace FoodSense.API.Extensions;

public static class OpenFoodFactProductExtensions
{
    /// <summary>Maps an OpenFoodFacts candidate onto the search dropdown. The result has no local
    /// product id: it is persisted only when the user adds it to the pantry.</summary>
    public static ProductSearchResultDto ToSearchResult(this OpenFoodFactProduct openFoodFactProduct)
    {
        return new ProductSearchResultDto
        {
            ProductId = 0,
            Barcode = string.IsNullOrWhiteSpace(openFoodFactProduct.Code) ? null : openFoodFactProduct.Code,
            Name = openFoodFactProduct.Name,
            Brand = openFoodFactProduct.Brand,
            PackageSize = openFoodFactProduct.Quantity,
            ImageUrl = openFoodFactProduct.ThumbnailUrl(),
            Nutrients = openFoodFactProduct.Nutrients.ToNutrientsDto(),
            Source = ProductSource.OpenFoodFacts
        };
    }

    /// <summary>Maps an OpenFoodFacts product onto the product form (<c>Id</c> = 0 keeps it unpersisted).</summary>
    public static ProductDto ToProductDto(this OpenFoodFactProduct openFoodFactProduct)
    {
        return new ProductDto
        {
            Barcode = string.IsNullOrWhiteSpace(openFoodFactProduct.Code) ? null : openFoodFactProduct.Code,
            ProductName = openFoodFactProduct.Name,
            Brands = openFoodFactProduct.Brand,
            ProductQuantity = openFoodFactProduct.Quantity,
            FrontImageUrl = openFoodFactProduct.ThumbnailUrl(),
            Nutrients = openFoodFactProduct.Nutrients.ToNutrientsDto()
        };
    }

    private static NutrientsDto ToNutrientsDto(this OpenFoodFactNutrition nutrients)
    {
        return new NutrientsDto
        {
            EnergyKcal = nutrients.Calories,
            Fat = nutrients.Fat,
            SaturatedFat = nutrients.SaturatedFat,
            Carbohydrates = nutrients.Carbohydrates,
            Sugars = nutrients.Sugars,
            Proteins = nutrients.Proteins,
            Salt = nutrients.Salt
        };
    }

    /// <summary>Search results fall back to the full-size image when no thumbnail is published.</summary>
    private static string ThumbnailUrl(this OpenFoodFactProduct openFoodFactProduct)
        => string.IsNullOrWhiteSpace(openFoodFactProduct.ImageSmallUrl)
            ? openFoodFactProduct.ImageUrl
            : openFoodFactProduct.ImageSmallUrl;
}

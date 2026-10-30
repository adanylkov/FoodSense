using FoodSense.API.Data.Models;
using FoodSense.API.Data.ValueObjects;

namespace FoodSense.API.Extensions;

public static class OpenFoodFactProductExtensions
{
    public static Product MapFromOpenFoodFact(this OpenFoodFactProduct openFoodFactProduct, string barcode)
    {
        return new Product
        {
            Barcode = barcode,
            Name = openFoodFactProduct.Name,
            FrontImageUrl = openFoodFactProduct.ImageUrl,
            Nutrients = new Nutrients
            {
                Carbohydrates = openFoodFactProduct.Nutrients.Carbohydrates,
                Kcal = openFoodFactProduct.Nutrients.Calories,
                Fat = openFoodFactProduct.Nutrients.Fat,
                Proteins = openFoodFactProduct.Nutrients.Proteins,
                Salt = openFoodFactProduct.Nutrients.Salt,
                SaturatedFat = openFoodFactProduct.Nutrients.SaturatedFat,
                Sugars = openFoodFactProduct.Nutrients.Sugars
            }
        };
    }
}

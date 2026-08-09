using FoodSense.API.Data.Models;

namespace FoodSense.API.Extensions;

public static class OpenFoodFactProductExtensions
{
    public static Product MapFromOpenFoodFact(this OpenFoodFactProduct openFoodFactProduct, string barcode)
    {
        return new Product
        {
            Barcode = barcode,
            ProductName = openFoodFactProduct.Name,
            FrontImageUrl = openFoodFactProduct.ImageUrl,
            Nutrients = new Nutrients
            {
                Carbohydrates = openFoodFactProduct.Nutrients.Carbohydrates,
                EnergyKcal = openFoodFactProduct.Nutrients.Calories,
                Fat = openFoodFactProduct.Nutrients.Fat,
                Proteins = openFoodFactProduct.Nutrients.Proteins,
                Salt = openFoodFactProduct.Nutrients.Salt,
                SaturatedFat = openFoodFactProduct.Nutrients.SaturatedFat,
                Sugars = openFoodFactProduct.Nutrients.Sugars
            }
        };
    }
}

using FoodSense.API.Data.Models;
using FoodSense.Common.Models;

namespace FoodSense.API.Extensions;

public static class ProductDtoExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Barcode = product.Barcode,
            ProductName = product.ProductName,
            FrontImageUrl = product.FrontImageUrl,
            Nutrients = new NutrientsDto
            {
                EnergyKcal = product.Nutrients.EnergyKcal,
                Fat = product.Nutrients.Fat,
                SaturatedFat = product.Nutrients.SaturatedFat,
                Carbohydrates = product.Nutrients.Carbohydrates,
                Sugars = product.Nutrients.Sugars,
                Proteins = product.Nutrients.Proteins,
                Salt = product.Nutrients.Salt
            }
        };
    }

    public static IEnumerable<ProductDto> ToProductDtos(this IEnumerable<Product> products)
    {
        return products.Select(p => p.ToDto());
    }
}
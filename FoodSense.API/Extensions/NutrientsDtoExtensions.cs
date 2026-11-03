using FoodSense.API.Data.ValueObjects;
using FoodSense.Common.Models;

namespace FoodSense.API.Extensions;

/// <summary>Single mapping between the API nutrients value object and the shared DTO.
/// The DTO uses <c>EnergyKcal</c> while the value object uses <c>Kcal</c>.</summary>
public static class NutrientsDtoExtensions
{
    public static Nutrients ToNutrients(this NutrientsDto nutrients)
    {
        return new Nutrients
        {
            Kcal = nutrients.EnergyKcal,
            Fat = nutrients.Fat,
            SaturatedFat = nutrients.SaturatedFat,
            Carbohydrates = nutrients.Carbohydrates,
            Sugars = nutrients.Sugars,
            Proteins = nutrients.Proteins,
            Salt = nutrients.Salt
        };
    }

    public static NutrientsDto ToNutrientsDto(this Nutrients nutrients)
    {
        return new NutrientsDto
        {
            EnergyKcal = nutrients.Kcal,
            Fat = nutrients.Fat,
            SaturatedFat = nutrients.SaturatedFat,
            Carbohydrates = nutrients.Carbohydrates,
            Sugars = nutrients.Sugars,
            Proteins = nutrients.Proteins,
            Salt = nutrients.Salt
        };
    }
}

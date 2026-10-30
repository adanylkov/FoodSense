using FoodSense.API.Data.ValueObjects;
using FoodSense.Common.Models;
using FoodSense.Common.Requests;

namespace FoodSense.API.Data.Models
{
    public static class FoodLogEntryFactory
    {
        public static FoodLogEntry Create(Product product, AddFoodLogRequest request)
        {
            return new FoodLogEntry(product, request.MealType, NutritionSnapshot.FromNutrients(product.Nutrients, request.Quantity), request.Quantity);
        }

        public static FoodLogEntry Create(Product product, EditFoodLogRequest request)
        {
            return new FoodLogEntry(product, request.MealType, NutritionSnapshot.FromNutrients(product.Nutrients, request.Quantity), request.Quantity);
        }

        public static FoodLogEntry Create(Product product, MealType mealType, decimal quantity)
        {
            var nutritionSnapshot = NutritionSnapshot.FromNutrients(product.Nutrients, quantity);

            return new FoodLogEntry(product, mealType, nutritionSnapshot, quantity);
        }
    }
}
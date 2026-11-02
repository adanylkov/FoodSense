using System.Text.Json;
using FluentAssertions;
using FoodSense.Common.Models;

namespace FoodSense.BlazorFrontend.Tests;

/// <summary>
/// The Frontend no longer references FoodSense.API, so the only link between the API
/// response and the Frontend view models is JSON. These tests lock that contract:
/// the JSON below was captured from the running API
/// (GET api/CalorieTracker/GetEntries?date=2026-10-30, GET api/CalorieTracker/GetGoals).
/// </summary>
public class FoodLogEntryDtoTests
{
    private const string EntriesJson = """
    [
      {
        "id": 11,
        "userId": null,
        "date": "2026-10-30T00:00:00",
        "productId": 1055,
        "product": {
          "id": 1055,
          "barcode": "5907809285964",
          "name": "Twarog klinek chudy",
          "frontImageUrl": "https://images.openfoodfacts.org/images/products/590/780/928/5964/front_en.3.400.jpg",
          "nutrients": { "kcal": 86, "fat": 0.4, "saturatedFat": 0.3, "carbohydrates": 3.7, "sugars": 3, "proteins": 17, "salt": 0.0975 },
          "pantryItems": [],
          "categoryId": 4,
          "category": null
        },
        "mealType": 1,
        "nutritions": { "calories": 107.5, "protein": 21.25, "fat": 0.5, "carbs": 4.625 },
        "consumedQuantity": 125.0
      },
      {
        "id": 13,
        "userId": null,
        "date": "2026-10-30T00:00:00",
        "productId": 1055,
        "product": { "id": 1055, "name": "Twarog klinek chudy", "pantryItems": [] },
        "mealType": 2,
        "nutritions": { "calories": 86, "protein": 17, "fat": 0.4, "carbs": 3.7 },
        "consumedQuantity": 100.0
      }
    ]
    """;

    private const string GoalsJson = """
    { "id": 1, "userId": "1", "targetCalories": 1669.00, "targetProtein": 140.00, "targetFat": 53.00, "targetCarbs": 158.00 }
    """;

    // Same defaults as HttpClient.GetFromJsonAsync uses (JsonSerializerDefaults.Web).
    private static readonly JsonSerializerOptions WebOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public void DeserializeEntries_ShouldMapEveryFieldTheFrontendReads()
    {
        var entries = JsonSerializer.Deserialize<List<FoodLogEntryDto>>(EntriesJson, WebOptions);

        entries.Should().NotBeNull();
        entries.Should().HaveCount(2);

        var first = entries[0];
        first.Id.Should().Be(11);
        first.Product.Name.Should().Be("Twarog klinek chudy");
        first.MealType.Should().Be(MealType.Breakfast);
        first.Nutritions.Calories.Should().Be(107.5);
        first.Nutritions.Protein.Should().Be(21.25);
        first.Nutritions.Fat.Should().Be(0.5);
        first.Nutritions.Carbs.Should().Be(4.625);
        first.ConsumedQuantity.Should().Be(125m);
    }

    [Fact]
    public void DeserializeEntries_ShouldSupportSummingNutrientsLikeTheListViewDoes()
    {
        var entries = JsonSerializer.Deserialize<List<FoodLogEntryDto>>(EntriesJson, WebOptions)!;

        var totalCalories = entries.Sum(e => e.Nutritions.Calories);

        totalCalories.Should().Be(193.5);
    }

    [Fact]
    public void DeserializeGoals_ShouldMapAllTargets()
    {
        var goals = JsonSerializer.Deserialize<UserNutritionalGoalsDto>(GoalsJson, WebOptions);

        goals.Should().NotBeNull();
        goals!.TargetCalories.Should().Be(1669m);
        goals.TargetProtein.Should().Be(140m);
        goals.TargetFat.Should().Be(53m);
        goals.TargetCarbs.Should().Be(158m);
    }

    [Fact]
    public void NutritionSnapshot_ShouldSupportWithExpressionsUsedByTheEditModal()
    {
        var snapshot = new NutritionSnapshotDto(107.5, 21.25, 0.5, 4.625);

        var rounded = snapshot with { Calories = Math.Round(snapshot.Calories, 1) };

        rounded.Calories.Should().Be(107.5);
        rounded.Protein.Should().Be(21.25);
        rounded.Should().NotBeSameAs(snapshot);
    }
}

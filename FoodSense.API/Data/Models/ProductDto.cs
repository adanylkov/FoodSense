using System.Text.Json.Serialization;

public class OpenFoodFactsResponse
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("product")]
    public ProductDto? Product { get; set; }
}

public class ProductDto
{
    [JsonPropertyName("product_name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("brands")]
    public string Brand { get; set; } = string.Empty;

    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    public string Quantity { get; set; } = string.Empty;

    [JsonPropertyName("nutriments")]
    public NutritionDto Nutrients { get; set; } = new();
}

public class NutritionDto
{
    [JsonPropertyName("energy-kcal_100g")]
    public double Calories { get; set; }

    [JsonPropertyName("fat_100g")]
    public double Fat { get; set; }

    [JsonPropertyName("saturated-fat_100g")]
    public double SaturatedFat { get; set; }

    [JsonPropertyName("carbohydrates_100g")]
    public double Carbohydrates { get; set; }

    [JsonPropertyName("sugars_100g")]
    public double Sugars { get; set; }

    [JsonPropertyName("proteins_100g")]
    public double Proteins { get; set; }

    [JsonPropertyName("salt_100g")]
    public double Salt { get; set; }
}
namespace FoodSense.Common.Models;

public sealed record NutrientsDto
{
    public double EnergyKcal { get; set; }
    public double Fat { get; set; }
    public double SaturatedFat { get; set; }
    public double Carbohydrates { get; set; }
    public double Sugars { get; set; }
    public double Proteins { get; set; }
    public double Salt { get; set; }
}

public sealed record ProductDto
{
    public static readonly string PlaceholderImagePath = "/images/product-placeholder.svg";
    public int Id { get; set; }
    public string? Barcode { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Brands { get; set; } = string.Empty;
    public string ProductQuantity { get; set; } = string.Empty;
    public string? FrontImageUrl { get; set; }
    public NutrientsDto Nutrients { get; set; } = new();
    public IEnumerable<PantryItemDto> PantryItems { get; set; } = new List<PantryItemDto>();

    public ProductDto CloneProduct()
    {
        return new ProductDto
        {
            Id = Id,
            Barcode = Barcode,
            ProductName = ProductName,
            Brands = Brands,
            ProductQuantity = ProductQuantity,
            FrontImageUrl = FrontImageUrl ?? PlaceholderImagePath,
            Nutrients = new NutrientsDto
            {
                EnergyKcal = Nutrients.EnergyKcal,
                Fat = Nutrients.Fat,
                SaturatedFat = Nutrients.SaturatedFat,
                Carbohydrates = Nutrients.Carbohydrates,
                Sugars = Nutrients.Sugars,
                Proteins = Nutrients.Proteins,
                Salt = Nutrients.Salt
            },
            PantryItems = new List<PantryItemDto>(PantryItems)
        };
    }
}
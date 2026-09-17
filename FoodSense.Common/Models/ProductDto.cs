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
    public int Id { get; set; }
    public string Barcode { get; set; } = string.Empty;
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
            Id = this.Id,
            Barcode = this.Barcode,
            ProductName = this.ProductName,
            Brands = this.Brands,
            ProductQuantity = this.ProductQuantity,
            FrontImageUrl = this.FrontImageUrl,
            Nutrients = new NutrientsDto
            {
                EnergyKcal = this.Nutrients.EnergyKcal,
                Fat = this.Nutrients.Fat,
                SaturatedFat = this.Nutrients.SaturatedFat,
                Carbohydrates = this.Nutrients.Carbohydrates,
                Sugars = this.Nutrients.Sugars,
                Proteins = this.Nutrients.Proteins,
                Salt = this.Nutrients.Salt
            },
            PantryItems = new List<PantryItemDto>(this.PantryItems)
        };
    }
}
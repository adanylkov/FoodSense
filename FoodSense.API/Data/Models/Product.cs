namespace FoodSense.API.Data.Models;

public class Product : OpenFoodFactsCSharp.Models.Product
{
    public int Id { get; set; }
    public string Barcode { get; set; } = string.Empty;
}

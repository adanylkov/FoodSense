using System.ComponentModel.DataAnnotations;

namespace FoodSense.API.Data.Models;

public class Product : OpenFoodFactsCSharp.Models.Product
{
    public int Id { get; set; }

    [MaxLength(450)]
    public string Barcode { get; set; } = string.Empty;
}

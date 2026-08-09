namespace FoodSense.API.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string FrontImageUrl { get; set; } = string.Empty;
        public Nutrients Nutrients { get; set; } = new();
        public ICollection<PantryItem> PantryItems { get; set; } = new List<PantryItem>();
    }

    public class Nutrients
    {
        public double EnergyKcal { get; set; }
        public double Fat { get; set; }
        public double SaturatedFat { get; set; }
        public double Carbohydrates { get; set; }
        public double Sugars { get; set; }
        public double Proteins { get; set; }
        public double Salt { get; set; }
    }
}

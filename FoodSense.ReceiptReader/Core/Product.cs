namespace FoodSense.ReceiptReader.Core;

public record Product
{
    public required string Name { get; init; }
    public int Quantity { get; set; } = 1;
    public decimal Price { get; set; }
}

public record Receipt
{
    public IList<Product> Products { get; set; } = [];
    public decimal Total { get; set; }
    public DateTime Date { get; set; }
    public string? StoreLocation { get; set; }
}


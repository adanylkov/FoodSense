public sealed record ProductMappingDto
{
    public string ProductName { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public int ProductId { get; set; }
}
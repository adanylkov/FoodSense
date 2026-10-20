using System;
using FoodSense.ReceiptReader;

namespace FoodSense.BlazorFrontend.Core;

public sealed class ReceiptPreviewItem
{
    public required ReceiptItem Item { get; init; }
    public bool IsSelected { get; set; } = true;
    public bool IsMatched { get; set; } = false;
    public string Name => HasCandidate ? ProductName! : Item.Name;
    public bool HasCandidate => ProductIdCandidate.HasValue && !string.IsNullOrWhiteSpace(ProductName);
    public string? BarcodeCandidate { get; set; }
    public int? ProductIdCandidate { get; set; }
    public string? ProductName { get; set; }
    public string? ProductNameCandidate { get; set; }
}

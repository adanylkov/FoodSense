using System;
using FoodSense.ReceiptReader;

namespace FoodSense.BlazorFrontend.Core;

public sealed class ReceiptPreviewItem
{
    public required ReceiptItem Item { get; init; }
    public bool IsSelected { get; set; } = true;
    public bool IsMatched { get; set; } = false;
    public string Name => HasCandidate ? ProductName! : Item.Name;
    public bool HasCandidate => !string.IsNullOrWhiteSpace(BarcodeCandidate) && !string.IsNullOrWhiteSpace(ProductName);
    public string? BarcodeCandidate { get; set; }
    public int? ProductIdCandidate { get; set; }
    public string? ProductName { get; set; }
}


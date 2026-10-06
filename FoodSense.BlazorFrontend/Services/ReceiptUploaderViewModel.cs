using System;
using System.Diagnostics;
using FoodSense.BlazorFrontend.Core;
using FoodSense.Common.Models;
using FoodSense.ReceiptReader;
using FoodSense.Services;
using Microsoft.AspNetCore.Components;

namespace FoodSense.BlazorFrontend.Services;

public class ReceiptUploaderViewModel
{
    private readonly IReceiptReaderFactory _receiptReaderFactory;
    private readonly IMatchApiService _matchApiService;
    private readonly IProductApiService _productApiService;

    public ReceiptUploaderViewModel(IReceiptReaderFactory receiptReaderFactory, IMatchApiService matchApiService, IProductApiService productApiService)
    {
        _receiptReaderFactory = receiptReaderFactory;
        _matchApiService = matchApiService;
        _productApiService = productApiService;
    }

    public async Task<List<ReceiptPreviewItem>> ReadReceiptItems(Stream stream, string fileName)
    {
        var reader = _receiptReaderFactory.GetReader(Path.GetExtension(fileName));
        var items = await reader.ReadAsync(stream);
        var receiptPreviewItems = items
            .Select(item => new ReceiptPreviewItem { Item = item, IsSelected = true })
            .ToList();
        return receiptPreviewItems;
    }

    public async Task<Result> MatchProductsAsync(IList<ReceiptPreviewItem> receiptPreviewItems)
    {
        try
        {
            var names = receiptPreviewItems
                .Where(i => i.IsSelected)
                .Select(i => i.Item.Name)
                .ToList();

            if (names.Count > 0)
            {
                ApiResponse<IEnumerable<ProductMappingDto>>? matchedProducts = await _matchApiService.MatchProductsAsync(names);
                if (matchedProducts is not null)
                {
                    foreach (var (i, item) in matchedProducts.Data?.Index() ?? [])
                    {
                        if (receiptPreviewItems.Count <= i) break;
                        if (string.IsNullOrWhiteSpace(item.Barcode)) continue;

                        var receiptPreviewItem = receiptPreviewItems[i];

                        receiptPreviewItem.IsMatched = true;
                        receiptPreviewItem.BarcodeCandidate = item.Barcode;
                        receiptPreviewItem.ProductName = item.ProductName;
                    }

                    return Result.Success();
                }
            }
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error matching products: {ex.Message}");
        }

        return Result.Failure("No products matched.");
    }

    internal async Task<Result> AddProductMappingAsync(ReceiptPreviewItem item)
    {
        Debug.Assert(item.HasCandidate, "Item must be matched with a candidate before adding a product mapping.");
        Debug.Assert(!string.IsNullOrWhiteSpace(item.BarcodeCandidate), "Item must have a barcode before adding a product mapping.");
        Debug.Assert(!string.IsNullOrWhiteSpace(item.ProductName), "Item must have a product name before adding a product mapping.");
        Debug.Assert(item.ProductIdCandidate.HasValue, "Item must have a product ID before adding a product mapping.");

        var productMapping = new ProductMappingDto
        {
            Barcode = item.BarcodeCandidate,
            ProductName = item.Item.Name,
            ProductId = item.ProductIdCandidate.Value
        };

        try
        {
            if (await _matchApiService.AddProductMappingAsync(productMapping) is false)
            {
                return Result.Failure($"Failed to add product mapping for {item.Item.Name}.");
            }
            else {
                item.IsMatched = true;
            }
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error adding product mapping: {ex.Message}");
        }

        return Result.Success();
    }

    internal async Task<Result> ConfirmReceiptItemsAsync(IList<ReceiptPreviewItem> receiptPreviewItems)
    {
        if (receiptPreviewItems.Count == 0) return Result.Failure("No receipt items to confirm.");

        var itemsToAdd = receiptPreviewItems
            .Where(i => i is { IsSelected: true, IsMatched: true });

        foreach (var receiptItem in itemsToAdd)
        {
            Debug.Assert(receiptItem.HasCandidate, "Receipt item must have a candidate before adding to pantry.");
            Debug.Assert(!string.IsNullOrWhiteSpace(receiptItem.BarcodeCandidate), "Receipt item must have a barcode before adding to pantry.");

            try
            {
                var product = await _productApiService.GetProductByBarcodeAsync(receiptItem.BarcodeCandidate!);
                if (product is not null)
                {

                    await _productApiService.CreatePantryItemAsync(product.Id, new PantryItemDto {
                        ProductId = product.Id,
                        Quantity = receiptItem.Item.Quantity,
                    });
                }
                else
                {
                    return Result.Failure($"Product with barcode {receiptItem.BarcodeCandidate} not found.");
                }
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error adding product to pantry: {ex.Message}");
            }
        }

        receiptPreviewItems.Clear();

        return Result.Success();
    }

    internal async Task<Result> SetBarcodeAsync(string? barcode, ReceiptPreviewItem item)
    {
        if (!string.IsNullOrWhiteSpace(barcode))
        {
            item.BarcodeCandidate = barcode;
            try {
                var product = await _productApiService.GetProductByBarcodeAsync(barcode);
                if (product is not null) {

                    item.BarcodeCandidate = barcode;
                    item.ProductName = product.ProductName;
                    item.ProductIdCandidate = product.Id;
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error fetching product by barcode: {ex.Message}");
            }
        }

        return Result.Failure("Barcode is null or empty.");
    }
}

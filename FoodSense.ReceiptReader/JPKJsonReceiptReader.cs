using System.Globalization;
using System.Text.Json;
using FoodSense.API.Data.Models;
using FoodSense.Common.Models;

namespace FoodSense.ReceiptReader;

public class JPKJsonReceiptReader : IReceiptReader
{
  private readonly CultureInfo _culture;

  public JPKJsonReceiptReader(CultureInfo? cultureInfo = null)
  {
    _culture = cultureInfo ?? new CultureInfo("pl-PL");
  }

  public static IEnumerable<BodyItem> Read(string json)
  {
    return ReadBodyItems(json);
  }

  public IEnumerable<Product> ReadProducts(string json)
  {
    return ReadPantryItems(json).Select(item => item.Product);
  }

  public async Task<IEnumerable<ProductDto>> ReadAsync(Stream stream)
  {
    ArgumentNullException.ThrowIfNull(stream);

    using var reader = new StreamReader(stream, leaveOpen: true);
    var json = await reader.ReadToEndAsync();

    if (string.IsNullOrWhiteSpace(json))
      return [];

    var bodyItems = ReadBodyItems(json).Where(b => b.SellLine is not null).ToList();
    return bodyItems.Select(item => new ProductDto
    {
      ProductName = ExtractName(item.SellLine!.Name),
      Barcode = item.Barcode?.Data ?? string.Empty,
    });
  }

  private static IEnumerable<BodyItem> ReadBodyItems(string json)
  {
    if (string.IsNullOrWhiteSpace(json))
      return Enumerable.Empty<BodyItem>();

    var receiptRoot = JsonSerializer.Deserialize<ReceiptRoot>(json);
    return receiptRoot?.Body ?? Enumerable.Empty<BodyItem>();
  }

  private IEnumerable<PantryItem> ReadPantryItems(string json)
  {
    return ReadBodyItems(json)
      .Where(b => b.SellLine != null)
      .Select(MapSellLineToPantryItem);
  }

  private PantryItem MapSellLineToPantryItem(BodyItem bodyItem)
  {
    var sell = bodyItem.SellLine!;
    var productName = ExtractName(sell.Name);
    var product = new Product
    {
      Barcode = ExtractBarcode(bodyItem),
      ProductName = productName,
      FrontImageUrl = string.Empty,
      Nutrients = new Nutrients(),
      PantryItems = []
    };

    var pantryItem = new PantryItem
    {
      Product = product,
      ProductId = product.Id,
      Quantity = ParseQuantityDecimal(sell.Quantity),
      AddedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };

    product.PantryItems = [pantryItem];
    return pantryItem;
  }

  private static string ExtractBarcode(BodyItem bodyItem)
  {
    return bodyItem.Barcode?.Data ?? string.Empty;
  }

  private static string ExtractName(string? rawName)
  {
    if (string.IsNullOrWhiteSpace(rawName))
      return string.Empty;

    var trimmed = rawName.Trim();
    return trimmed.Length > 0 ? trimmed.Substring(0, Math.Max(0, trimmed.Length - 1)).Trim() : string.Empty;
  }

  private decimal ParseQuantityDecimal(string? quantityStr)
  {
    if (string.IsNullOrWhiteSpace(quantityStr))
      return 0m;

    if (decimal.TryParse(quantityStr, NumberStyles.Float, _culture, out var value))
      return value;

    if (decimal.TryParse(quantityStr, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
      return value;

    return 0m;
  }
}

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

  public async Task<IEnumerable<ReceiptItem>> ReadAsync(Stream stream)
  {
    ArgumentNullException.ThrowIfNull(stream);

    using var reader = new StreamReader(stream, leaveOpen: true);
    var json = await reader.ReadToEndAsync();

    if (string.IsNullOrWhiteSpace(json))
      return [];

    return ReadBodyItems(json)
      .Where(b => b.SellLine is { Name: not null })
      .Select(item => new ReceiptItem
      {
        Name = item.SellLine!.Name!.Trim(), 
        Quantity = decimal.TryParse(item.SellLine.Quantity, NumberStyles.Any, _culture, out var quantity) ? quantity : 0m
      });
  }

  private static IEnumerable<BodyItem> ReadBodyItems(string json)
  {
    if (string.IsNullOrWhiteSpace(json))
      return Enumerable.Empty<BodyItem>();

    var receiptRoot = JsonSerializer.Deserialize<ReceiptRoot>(json);
    return receiptRoot?.Body ?? Enumerable.Empty<BodyItem>();
  }
}

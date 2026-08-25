using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using FoodSense.API.Data.Models;

namespace FoodSense.ReceiptReader;

public class JPKJsonReceiptReader
{
  private static readonly CultureInfo PolishCulture = new("pl-PL");
  private readonly List<Product> _products = [];
  private readonly List<PantryItem> _pantryItems = [];

  public IReadOnlyList<Product> Products => _products;
  public IReadOnlyList<PantryItem> PantryItems => _pantryItems;

  public JPKJsonReceiptReader(string json)
  {
    var pantryItems = ReadPantryItems(json).ToList();
    _pantryItems = pantryItems;
    _products = pantryItems.Select(item => item.Product).ToList();
  }

  public static IEnumerable<BodyItem> Read(string json)
  {
    return ReadBodyItems(json);
  }

  private IEnumerable<Product> ReadProducts(string json)
  {
    return ReadPantryItems(json).Select(item => item.Product);
  }

  private static IEnumerable<BodyItem> ReadBodyItems(string json)
  {
    if (string.IsNullOrWhiteSpace(json))
      return Enumerable.Empty<BodyItem>();

    var receiptRoot = JsonSerializer.Deserialize<ReceiptRoot>(json);
    return receiptRoot?.Body ?? Enumerable.Empty<BodyItem>();
  }

  private static IEnumerable<PantryItem> ReadPantryItems(string json)
  {
    return ReadBodyItems(json)
      .Where(b => b.SellLine != null)
      .Select(MapSellLineToPantryItem);
  }

  private static PantryItem MapSellLineToPantryItem(BodyItem bodyItem)
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

  private static decimal ParseQuantityDecimal(string? quantityStr)
  {
    if (string.IsNullOrWhiteSpace(quantityStr))
      return 0m;

    if (decimal.TryParse(quantityStr, NumberStyles.Float, PolishCulture, out var value))
      return value;

    if (decimal.TryParse(quantityStr, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
      return value;

    return 0m;
  }
}

public class ReceiptRoot
{
  [JsonPropertyName("protoVersion")]
  public string? ProtoVersion { get; set; }

  [JsonPropertyName("IDZ")]
  public string? Idz { get; set; }

  [JsonPropertyName("deviceType")]
  public int DeviceType { get; set; }

  [JsonPropertyName("printed")]
  public bool Printed { get; set; }

  [JsonPropertyName("data")]
  public string? Data { get; set; }

  [JsonPropertyName("document")]
  public Document? Document { get; set; }

  [JsonPropertyName("header")]
  public List<HeaderItem>? Header { get; set; }

  [JsonPropertyName("body")]
  public List<BodyItem>? Body { get; set; }

  [JsonPropertyName("sign")]
  public string? Sign { get; set; }
}

public class Document
{
  [JsonPropertyName("naglowek")]
  public Naglowek? Naglowek { get; set; }

  [JsonPropertyName("podmiot1")]
  public Podmiot1? Podmiot1 { get; set; }

  [JsonPropertyName("paragon")]
  public Paragon? Paragon { get; set; }
}

public class Naglowek
{
  [JsonPropertyName("wersja")]
  public string? Wersja { get; set; }

  [JsonPropertyName("dataJPK")]
  public DateTimeOffset DataJpk { get; set; }
}

public class Podmiot1
{
  [JsonPropertyName("NIP")]
  public string? Nip { get; set; }

  [JsonPropertyName("nazwaPod")]
  public string? NazwaPod { get; set; }

  [JsonPropertyName("adresPod")]
  public AdresPod? AdresPod { get; set; }

  [JsonPropertyName("nrUnik")]
  public string? NrUnik { get; set; }

  [JsonPropertyName("nrFabr")]
  public string? NrFabr { get; set; }

  [JsonPropertyName("nrEwid")]
  public string? NrEwid { get; set; }
}

public class AdresPod
{
  [JsonPropertyName("kodPoczt")]
  public string? KodPoczt { get; set; }

  [JsonPropertyName("miejsce")]
  public string? Miejsce { get; set; }

  [JsonPropertyName("ulica")]
  public string? Ulica { get; set; }
}

public class Paragon
{
  [JsonPropertyName("JPKID")]
  public long JpkId { get; set; }

  [JsonPropertyName("pamiecChr")]
  public int PamiecChr { get; set; }

  [JsonPropertyName("nrDok")]
  public int NrDok { get; set; }

  [JsonPropertyName("pozycje")]
  public List<Pozycja>? Pozycje { get; set; }

  [JsonPropertyName("stPTU")]
  public List<StPtu>? StPtu { get; set; }

  [JsonPropertyName("podsum")]
  public Podsum? Podsum { get; set; }

  [JsonPropertyName("total")]
  public Total? Total { get; set; }

  [JsonPropertyName("opak")]
  public Opak? Opak { get; set; }

  [JsonPropertyName("platnosci")]
  public List<Platnosc>? Platnosci { get; set; }

  [JsonPropertyName("zakSprzed")]
  public DateTimeOffset ZakSprzed { get; set; }

  [JsonPropertyName("nrKasy")]
  public string? NrKasy { get; set; }

  [JsonPropertyName("kasjer")]
  public string? Kasjer { get; set; }

  [JsonPropertyName("nrParag")]
  public int NrParag { get; set; }

  [JsonPropertyName("grafika")]
  public int Grafika { get; set; }
}

public class Pozycja
{
  [JsonPropertyName("towar")]
  public Towar? Towar { get; set; }
}

public class Towar
{
  [JsonPropertyName("brutto")]
  public int Brutto { get; set; }

  [JsonPropertyName("cena")]
  public int Cena { get; set; }

  [JsonPropertyName("idStPTU")]
  public string? IdStPtu { get; set; }

  [JsonPropertyName("ilosc")]
  public string? Ilosc { get; set; }

  [JsonPropertyName("nazwa")]
  public string? Nazwa { get; set; }

  [JsonPropertyName("oper")]
  public bool Oper { get; set; }

  [JsonPropertyName("rabat")]
  public Rabat? Rabat { get; set; }
}

public class Rabat
{
  [JsonPropertyName("opis")]
  public string? Opis { get; set; }

  [JsonPropertyName("wart")]
  public int Wart { get; set; }
}

public class StPtu
{
  [JsonPropertyName("id")]
  public string? Id { get; set; }

  [JsonPropertyName("wart")]
  public object? Wart { get; set; } // Can be int (e.g. 2300) or string (e.g. "ZW")
}

public class Podsum
{
  [JsonPropertyName("waluta")]
  public string? Waluta { get; set; }

  [JsonPropertyName("sumaBrutto")]
  public int SumaBrutto { get; set; }

  [JsonPropertyName("sumaPod")]
  public int SumaPod { get; set; }

  [JsonPropertyName("sumaOpust")]
  public int SumaOpust { get; set; }

  [JsonPropertyName("sumaNetto")]
  public List<SumaNetto>? SumaNetto { get; set; }
}

public class SumaNetto
{
  [JsonPropertyName("idStPTU")]
  public string? IdStPtu { get; set; }

  [JsonPropertyName("brutto")]
  public int Brutto { get; set; }

  [JsonPropertyName("vat")]
  public int Vat { get; set; }
}

public class Total
{
  [JsonPropertyName("zaplLzwrot")]
  public int ZaplLzwrot { get; set; }
}

public class Opak
{
  [JsonPropertyName("daneOpak")]
  public List<DaneOpak>? DaneOpak { get; set; }

  [JsonPropertyName("wart")]
  public int Wart { get; set; }
}

public class DaneOpak
{
  [JsonPropertyName("cena")]
  public int Cena { get; set; }

  [JsonPropertyName("ilosc")]
  public int Ilosc { get; set; }

  [JsonPropertyName("nazwa")]
  public string? Nazwa { get; set; }
}

public class Platnosc
{
  [JsonPropertyName("reszta")]
  public bool Reszta { get; set; }

  [JsonPropertyName("forma")]
  public string? Forma { get; set; }

  [JsonPropertyName("wart")]
  public int Wart { get; set; }

  [JsonPropertyName("nazwa")]
  public string? Nazwa { get; set; }
}

public class HeaderItem
{
  [JsonPropertyName("image")]
  public ImageData? Image { get; set; }

  [JsonPropertyName("headerText")]
  public HeaderText? HeaderText { get; set; }

  [JsonPropertyName("headerData")]
  public HeaderData? HeaderData { get; set; }
}

public class ImageData
{
  [JsonPropertyName("id")]
  public string? Id { get; set; }

  [JsonPropertyName("hash")]
  public string? Hash { get; set; }

  [JsonPropertyName("data")]
  public string? Data { get; set; } // Base64
}

public class HeaderText
{
  [JsonPropertyName("headerTextLines")]
  public string? HeaderTextLines { get; set; }
}

public class HeaderData
{
  [JsonPropertyName("tin")]
  public string? Tin { get; set; }

  [JsonPropertyName("docNumber")]
  public int DocNumber { get; set; }

  [JsonPropertyName("date")]
  public DateTimeOffset Date { get; set; }

  [JsonPropertyName("CPS")]
  public int Cps { get; set; }
}

public class BodyItem
{
  [JsonPropertyName("sellLine")]
  public SellLine? SellLine { get; set; }

  [JsonPropertyName("discountLine")]
  public DiscountLine? DiscountLine { get; set; }

  [JsonPropertyName("discountSummary")]
  public DiscountSummary? DiscountSummary { get; set; }

  [JsonPropertyName("vatSummary")]
  public VatSummary? VatSummary { get; set; }

  [JsonPropertyName("sumInCurrency")]
  public SumInCurrency? SumInCurrency { get; set; }

  [JsonPropertyName("section")]
  public Section? Section { get; set; }

  [JsonPropertyName("pack")]
  public Pack? Pack { get; set; }

  [JsonPropertyName("payment")]
  public Payment? Payment { get; set; }

  [JsonPropertyName("fiscalFooter")]
  public FiscalFooter? FiscalFooter { get; set; }

  [JsonPropertyName("addLine")]
  public AddLine? AddLine { get; set; }

  [JsonPropertyName("barcode")]
  public Barcode? Barcode { get; set; }

  [JsonPropertyName("sysNumber")]
  public SysNumber? SysNumber { get; set; }
}

public class SellLine
{
  [JsonPropertyName("name")]
  public string? Name { get; set; }

  [JsonPropertyName("vatId")]
  public string? VatId { get; set; }

  [JsonPropertyName("price")]
  public int Price { get; set; }

  [JsonPropertyName("total")]
  public int Total { get; set; }

  [JsonPropertyName("quantity")]
  public string? Quantity { get; set; }

  [JsonPropertyName("isStorno")]
  public bool IsStorno { get; set; }
}

public class DiscountLine
{
  [JsonPropertyName("base")]
  public int Base { get; set; }

  [JsonPropertyName("value")]
  public int Value { get; set; }

  [JsonPropertyName("isDiscount")]
  public bool IsDiscount { get; set; }

  [JsonPropertyName("isPercent")]
  public bool IsPercent { get; set; }

  [JsonPropertyName("isStorno")]
  public bool IsStorno { get; set; }

  [JsonPropertyName("vatId")]
  public string? VatId { get; set; }
}

public class DiscountSummary
{
  [JsonPropertyName("discounts")]
  public int Discounts { get; set; }
}

public class VatSummary
{
  [JsonPropertyName("currency")]
  public string? Currency { get; set; }

  [JsonPropertyName("vatRatesSummary")]
  public List<VatRateSummary>? VatRatesSummary { get; set; }
}

public class VatRateSummary
{
  [JsonPropertyName("vatId")]
  public string? VatId { get; set; }

  [JsonPropertyName("vatRate")]
  public int VatRate { get; set; }

  [JsonPropertyName("vatSale")]
  public int VatSale { get; set; }

  [JsonPropertyName("vatAmount")]
  public int VatAmount { get; set; }
}

public class SumInCurrency
{
  [JsonPropertyName("fiscalTotal")]
  public int FiscalTotal { get; set; }

  [JsonPropertyName("totalWithPacks")]
  public int TotalWithPacks { get; set; }

  [JsonPropertyName("currency")]
  public string? Currency { get; set; }

  [JsonPropertyName("printBig")]
  public bool PrintBig { get; set; }

  [JsonPropertyName("printable")]
  public bool Printable { get; set; }
}

public class Section
{
  [JsonPropertyName("type")]
  public int Type { get; set; }

  [JsonPropertyName("amount")]
  public int? Amount { get; set; }

  [JsonPropertyName("currency")]
  public string? Currency { get; set; }
}

public class Pack
{
  [JsonPropertyName("name")]
  public string? Name { get; set; }

  [JsonPropertyName("price")]
  public int Price { get; set; }

  [JsonPropertyName("quantity")]
  public string? Quantity { get; set; }

  [JsonPropertyName("total")]
  public int Total { get; set; }

  [JsonPropertyName("isNegative")]
  public bool IsNegative { get; set; }
}

public class Payment
{
  [JsonPropertyName("type")]
  public string? Type { get; set; }

  [JsonPropertyName("amount")]
  public int Amount { get; set; }

  [JsonPropertyName("name")]
  public string? Name { get; set; }

  [JsonPropertyName("currency")]
  public string? Currency { get; set; }
}

public class FiscalFooter
{
  [JsonPropertyName("billNumber")]
  public int BillNumber { get; set; }

  [JsonPropertyName("uniqueNumber")]
  public string? UniqueNumber { get; set; }

  [JsonPropertyName("cashNumber")]
  public string? CashNumber { get; set; }

  [JsonPropertyName("cashier")]
  public string? Cashier { get; set; }

  [JsonPropertyName("CPS")]
  public int Cps { get; set; }

  [JsonPropertyName("date")]
  public DateTimeOffset Date { get; set; }
}

public class AddLine
{
  [JsonPropertyName("id")]
  public int Id { get; set; }

  [JsonPropertyName("data")]
  public string? Data { get; set; }

  [JsonPropertyName("width")]
  public int Width { get; set; }

  [JsonPropertyName("CPS")]
  public int Cps { get; set; }
}

public class Barcode
{
  [JsonPropertyName("id")]
  public int Id { get; set; }

  [JsonPropertyName("data")]
  public string? Data { get; set; }
}

public class SysNumber
{
  [JsonPropertyName("data")]
  public string? Data { get; set; }

  [JsonPropertyName("width")]
  public int Width { get; set; }

  [JsonPropertyName("CPS")]
  public int Cps { get; set; }
}

using System.Text.Json;
using FluentAssertions;

namespace FoodSense.ReceiptReader.Tests;

public class JPKJsonReceiptReaderTest
{
    private readonly string SampleJson = File.ReadAllText(Path.Combine(Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName, "Receipts", "2605026145157738.json"));

    [Fact]
    public void Deserialize_ShouldMapHeaderDataCorrectly()
    {
        // Act
        var receipt = JsonSerializer.Deserialize<ReceiptRoot>(SampleJson);

        // Assert
        receipt.Should().NotBeNull();
        var headerData = receipt.Header?.FirstOrDefault(h => h.HeaderData != null)?.HeaderData;
        
        headerData.Should().NotBeNull();
        headerData.Tin.Should().Be("7791011327");
        headerData.DocNumber.Should().Be(369734);
        headerData.Date.Year.Should().Be(2026);
    }

    [Fact]
    public void Deserialize_ShouldMapSellLinesInBody()
    {
        // Act
        var receipt = JsonSerializer.Deserialize<ReceiptRoot>(SampleJson);
        var sellLines = receipt?.Body?.Where(b => b.SellLine != null).Select(b => b.SellLine!).ToList() ?? [];

        // Assert
        sellLines.Should().NotBeNull();
        sellLines.Should().HaveCount(3);
        sellLines[0].Name.Should().Contain("NektarBanRiviva1l");
        sellLines[0].Total.Should().Be(837);
        sellLines[2].Quantity.Should().Be("0,780"); // Testing decimal strings
    }

    [Fact]
    public void Deserialize_ShouldCaptureDiscountsCorrectly()
    {
        // Act
        var receipt = JsonSerializer.Deserialize<ReceiptRoot>(SampleJson);
        var discounts = receipt?.Body?.Where(b => b.DiscountLine != null).Select(b => b.DiscountLine).ToList() ?? [];

        // Assert
        discounts.Should().NotBeNull();
        discounts.Should().HaveCount(2);
        discounts[0]?.Value.Should().Be(279);
        discounts[1]?.Base.Should().Be(1793);
    }

    [Fact]
    public void Deserialize_ShouldMapVatSummary()
    {
        // Act
        var receipt = JsonSerializer.Deserialize<ReceiptRoot>(SampleJson);
        var vatSummary = receipt?.Body?.FirstOrDefault(b => b.VatSummary != null)?.VatSummary;

        // Assert
        vatSummary.Should().NotBeNull();
        if (vatSummary is null) return;
        vatSummary.VatRatesSummary.Should().NotBeNull();
        if (vatSummary.VatRatesSummary is null) return;

        vatSummary.Currency.Should().Be("PLN");
        vatSummary.VatRatesSummary[0].VatId.Should().Be("C");
        vatSummary.VatRatesSummary[0].VatAmount.Should().Be(82);
    }

    [Fact]
    public void Deserialize_ShouldMapBottleDeposits_Packs()
    {
        // Act
        var receipt = JsonSerializer.Deserialize<ReceiptRoot>(SampleJson);
        var pack = receipt?.Body?.FirstOrDefault(b => b.Pack != null)?.Pack;

        // Assert
        pack.Should().NotBeNull();
        if (pack is null) return;

        pack.Name.Should().Be("But Plastik kaucja");
        pack.Price.Should().Be(50);
        pack.Quantity.Should().Be("3");
    }

    [Fact]
    public void Deserialize_ShouldMapPaymentDetails()
    {
        // Act
        var receipt = JsonSerializer.Deserialize<ReceiptRoot>(SampleJson);
        var payment = receipt?.Body?.FirstOrDefault(b => b.Payment != null)?.Payment;

        // Assert
        payment.Should().NotBeNull();
        if (payment is null) return;

        payment.Name.Should().Be("Visa Debit 07 1");
        payment.Amount.Should().Be(1862); // 18.62 PLN
    }
}


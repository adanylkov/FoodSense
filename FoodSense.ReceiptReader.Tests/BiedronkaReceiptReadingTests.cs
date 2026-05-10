using FluentAssertions;

namespace FoodSense.ReceiptReader.Tests
{
    public class BiedronkaReceiptReadingTests
    {
        [Theory]
        [InlineData("1.pdf", 2)]
        [InlineData("2.pdf", 2)]
        [InlineData("3.pdf", 1)]
        public void ReadReceipt_ShouldReturnListOfProducts(string filename, int expectedImages)
        {
            var testPdfPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName, "Receipts", filename);
            var receiptPdf = File.OpenRead(testPdfPath);

            var images = BiedronkaReceiptReader.ReadImages(receiptPdf);
            images.Should().HaveCount(expectedImages);
            images.All(i => i.Length > 0).Should().BeTrue();
        }
    }
}

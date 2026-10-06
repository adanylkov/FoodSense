using FluentAssertions;
using FoodSense.BlazorFrontend.Core;
using FoodSense.BlazorFrontend.Services;
using FoodSense.Common.Models;
using FoodSense.ReceiptReader;
using FoodSense.Services;
using Moq;

namespace FoodSense.BlazorFrontend.Tests;

public class ReceiptUploaderViewModelTests
{
    [Fact]
    public async Task MatchProductsAsync_WithNoSelectedItems_ReturnsFailureWithoutMutatingItems()
    {
        var item = CreateItem("Milk", isSelected: false);
        var items = new List<ReceiptPreviewItem> { item };
        var matchApiService = new Mock<IMatchApiService>();
        var viewModel = CreateViewModel(matchApiService);

        var result = await viewModel.MatchProductsAsync(items);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("No products matched.");
        item.Should().BeEquivalentTo(CreateItem("Milk", isSelected: false));
        matchApiService.Verify(service => service.MatchProductsAsync(It.IsAny<IEnumerable<string>>()), Times.Never);
    }

    [Fact]
    public async Task MatchProductsAsync_WhenApiReturnsNull_ReturnsFailureWithoutMutatingItems()
    {
        var item = CreateItem("Milk");
        var items = new List<ReceiptPreviewItem> { item };
        var matchApiService = new Mock<IMatchApiService>();
        matchApiService
            .Setup(service => service.MatchProductsAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync((ApiResponse<IEnumerable<ProductMappingDto>>?)null);
        var viewModel = CreateViewModel(matchApiService);

        var result = await viewModel.MatchProductsAsync(items);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("No products matched.");
        item.Should().BeEquivalentTo(CreateItem("Milk"));
    }

    [Fact]
    public async Task MatchProductsAsync_WhenApiReturnsFewerResults_ReturnsSuccessAndMutatesOnlyMatchedItems()
    {
        var firstItem = CreateItem("Milk");
        var secondItem = CreateItem("Bread");
        var items = new List<ReceiptPreviewItem> { firstItem, secondItem };
        var matchApiService = new Mock<IMatchApiService>();
        matchApiService
            .Setup(service => service.MatchProductsAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new ApiResponse<IEnumerable<ProductMappingDto>>
            {
                Data =
                [
                    new ProductMappingDto { Barcode = "123", ProductName = "Matched milk" }
                ]
            });
        var viewModel = CreateViewModel(matchApiService);

        var result = await viewModel.MatchProductsAsync(items);

        result.IsSuccess.Should().BeTrue();
        firstItem.IsMatched.Should().BeTrue();
        firstItem.BarcodeCandidate.Should().Be("123");
        firstItem.ProductName.Should().Be("Matched milk");
        secondItem.Should().BeEquivalentTo(CreateItem("Bread"));
    }

    [Fact]
    public async Task MatchProductsAsync_WhenApiThrows_ReturnsFailureWithoutMutatingItems()
    {
        var item = CreateItem("Milk");
        var items = new List<ReceiptPreviewItem> { item };
        var matchApiService = new Mock<IMatchApiService>();
        matchApiService
            .Setup(service => service.MatchProductsAsync(It.IsAny<IEnumerable<string>>()))
            .ThrowsAsync(new InvalidOperationException("service unavailable"));
        var viewModel = CreateViewModel(matchApiService);

        var result = await viewModel.MatchProductsAsync(items);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Error matching products: service unavailable");
        item.Should().BeEquivalentTo(CreateItem("Milk"));
    }

    private static ReceiptUploaderViewModel CreateViewModel(Mock<IMatchApiService> matchApiService)
    {
        return new ReceiptUploaderViewModel(
            Mock.Of<IReceiptReaderFactory>(),
            matchApiService.Object,
            Mock.Of<IProductApiService>());
    }

    private static ReceiptPreviewItem CreateItem(string name, bool isSelected = true)
    {
        return new ReceiptPreviewItem
        {
            Item = new ReceiptItem { Name = name },
            IsSelected = isSelected
        };
    }
}

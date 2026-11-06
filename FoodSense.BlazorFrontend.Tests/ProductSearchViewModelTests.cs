using FluentAssertions;
using FoodSense.BlazorFrontend.Services;
using FoodSense.Common.Models;
using FoodSense.ViewModels;
using Moq;

namespace FoodSense.BlazorFrontend.Tests;

public class ProductSearchViewModelTests
{
    private readonly Mock<IProductApiService> _productApi = new();
    private readonly AlertService _alerts = new();
    private readonly ProductSearchViewModel _viewModel;

    public ProductSearchViewModelTests()
    {
        _viewModel = new ProductSearchViewModel(_alerts, _productApi.Object);
    }

    private static ProductSearchResultDto CreateResult(int id, string name = "Yogurt") => new()
    {
        ProductId = id,
        Name = name,
        Brand = "Brand",
        PackageSize = "100 g",
        Barcode = "7300120112345",
        ImageUrl = "/img/yogurt.jpg",
        Nutrients = new NutrientsDto { EnergyKcal = 60, Proteins = 3, Carbohydrates = 4, Fat = 0.5 },
        Source = id > 0 ? ProductSource.Local : ProductSource.OpenFoodFacts
    };

    private void SetupResults(params ProductSearchResultDto[] results) =>
        _productApi
            .Setup(a => a.SearchProductsAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(results.ToList());

    [Fact]
    public async Task SearchAsync_EmptyQuery_ReturnsNoResults_AndCallsNoApi()
    {
        await _viewModel.SearchAsync("   ");

        _viewModel.Results.Should().BeEmpty();
        _viewModel.IsOpen.Should().BeFalse();
        _productApi.Verify(a => a.SearchProductsAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task SearchAsync_TrimmedQuery_IsPassedWithLimit10()
    {
        SetupResults(CreateResult(1));

        await _viewModel.SearchAsync("  Yogurt  ");

        _viewModel.Query.Should().Be("Yogurt");
        _viewModel.Results.Should().HaveCount(1);
        _viewModel.IsOpen.Should().BeTrue();
        _productApi.Verify(a => a.SearchProductsAsync("Yogurt", 10), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ShouldNotPersistAnything()
    {
        SetupResults(CreateResult(0));

        await _viewModel.SearchAsync("Yogurt");

        _productApi.Verify(a => a.GetOrCreateProductAsync(It.IsAny<ProductDto>()), Times.Never);
    }

    [Fact]
    public async Task SearchAsync_RapidKeystrokes_OnlyLastQueryIsRequested()
    {
        SetupResults(CreateResult(1));

        // Fire three keystrokes without awaiting so the debounce timers overlap.
        var first = _viewModel.SearchAsync("a");
        var second = _viewModel.SearchAsync("ab");
        var third = _viewModel.SearchAsync("abc");

        await Task.WhenAll(first, second, third);

        _productApi.Verify(a => a.SearchProductsAsync("abc", 10), Times.Once);
        _productApi.Verify(a => a.SearchProductsAsync("a", It.IsAny<int>()), Times.Never);
        _productApi.Verify(a => a.SearchProductsAsync("ab", It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task SearchAsync_DoesNotLeaveBusyFlag_Set()
    {
        SetupResults(CreateResult(1));

        await _viewModel.SearchAsync("Yogurt");

        _viewModel.IsSearching.Should().BeFalse();
    }

    [Fact]
    public async Task SearchAsync_ApiFailure_SetsAlert_AndKeepsDropdownClosed()
    {
        _productApi
            .Setup(a => a.SearchProductsAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ThrowsAsync(new HttpRequestException("boom"));

        await _viewModel.SearchAsync("Yogurt");

        _alerts.Error.Should().Be("boom");
        _viewModel.IsOpen.Should().BeFalse();
        _viewModel.IsSearching.Should().BeFalse();
    }

    [Fact]
    public async Task Select_RemembersCandidate_AndClosesDropdown()
    {
        var result = CreateResult(0);
        SetupResults(result);

        await _viewModel.SearchAsync("Yogurt");
        _viewModel.Select(result);

        _viewModel.SelectedResult.Should().BeSameAs(result);
        _viewModel.Results.Should().BeEmpty();
        _viewModel.IsOpen.Should().BeFalse();
        _viewModel.Query.Should().Be("Yogurt");
    }

    [Fact]
    public async Task Clear_ResetsQueryResultsAndSelection()
    {
        var result = CreateResult(1);
        SetupResults(result);

        await _viewModel.SearchAsync("Yogurt");
        _viewModel.Select(result);

        _viewModel.Clear();

        _viewModel.Query.Should().BeEmpty();
        _viewModel.Results.Should().BeEmpty();
        _viewModel.SelectedResult.Should().BeNull();
        _viewModel.IsOpen.Should().BeFalse();
    }

    [Fact]
    public void ToProductDto_UnpersistedCandidate_KeepsZeroId()
    {
        var dto = CreateResult(0).ToProductDto();

        dto.Id.Should().Be(0);
        dto.ProductName.Should().Be("Yogurt");
        dto.Brands.Should().Be("Brand");
        dto.ProductQuantity.Should().Be("100 g");
        dto.FrontImageUrl.Should().Be("/img/yogurt.jpg");
    }
}

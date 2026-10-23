using FluentAssertions;
using FoodSense.Common.Models;
using FoodSense.Services;
using FoodSense.ViewModels;
using Moq;
using System.Net;

namespace FoodSense.BlazorFrontend.Tests;

public class ProductListViewModelTests : IDisposable
{
    private readonly Mock<IProductApiService> _api = new();
    private readonly AlertService _alerts = new();
    private readonly PantryViewModel _pantry;
    private readonly ProductFormViewModel _form;
    private readonly ProductDeletionViewModel _deletion;
    private readonly ProductListViewModel _viewModel;

    public ProductListViewModelTests()
    {
        _pantry = new PantryViewModel(_api.Object, _alerts);
        _form = new ProductFormViewModel(_api.Object, _alerts);
        _deletion = new ProductDeletionViewModel(_api.Object, _alerts);
        _viewModel = new ProductListViewModel(
            _api.Object,
            _pantry,
            _form,
            _deletion,
            _alerts);
    }

    public void Dispose() => _viewModel.Dispose();

    [Theory]
    [InlineData("pantry")]
    [InlineData("form")]
    [InlineData("deletion")]
    public async Task PublisherEvent_ReloadsProducts(string publisher)
    {
        SetupProducts(page: 1, pantryOnly: false, totalCount: 1, CreateProduct(1));

        await RaisePublisherEventAsync(publisher);

        _viewModel.Products.Select(p => p.Id).Should().Equal(1);
        _api.Verify(a => a.GetProductsAsync(1, _viewModel.Pagination.PageSize, false), Times.Once);
    }

    [Theory]
    [InlineData("pantry")]
    [InlineData("form")]
    [InlineData("deletion")]
    public async Task Dispose_DetachesPublisherEvents_AndCanBeRepeated(string publisher)
    {
        _viewModel.Dispose();
        _viewModel.Dispose();

        await RaisePublisherEventAsync(publisher);

        _api.Verify(a => a.GetProductsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
    }

    [Theory]
    [InlineData("pantry")]
    [InlineData("form")]
    [InlineData("deletion")]
    public async Task PublisherEvent_AfterReplacement_ReloadsOnlyReplacement(string publisher)
    {
        _viewModel.Dispose();
        using var replacement = new ProductListViewModel(_api.Object, _pantry, _form, _deletion, _alerts);
        SetupProducts(page: 1, pantryOnly: false, totalCount: 1, CreateProduct(1));

        await RaisePublisherEventAsync(publisher);

        _viewModel.Products.Should().BeEmpty();
        replacement.Products.Select(p => p.Id).Should().Equal(1);
        _api.Verify(a => a.GetProductsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public async Task SetPantryOnlyAsync_WhenValueChanges_LoadsFirstPageWithFilter()
    {
        SetupProducts(page: 3, pantryOnly: false, totalCount: 40, CreateProduct(1));
        SetupProducts(page: 1, pantryOnly: true, totalCount: 5, CreateProduct(2));
        await _viewModel.LoadAsync(3);
        _api.Invocations.Clear();

        await _viewModel.SetPantryOnlyAsync(true);

        _viewModel.PantryOnly.Should().BeTrue();
        _viewModel.Pagination.CurrentPage.Should().Be(1);
        _viewModel.Products.Select(p => p.Id).Should().Equal(2);
        _api.Verify(a => a.GetProductsAsync(1, _viewModel.Pagination.PageSize, true), Times.Once);
        _api.Verify(a => a.GetProductsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public async Task SetPantryOnlyAsync_WhenValueIsUnchanged_DoesNotCallApi()
    {
        await _viewModel.SetPantryOnlyAsync(false);

        _api.Verify(a => a.GetProductsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async Task LoadAsync_UpdatesPaginationFromResponse()
    {
        SetupProducts(page: 3, pantryOnly: false, totalCount: 30, CreateProduct(1));

        await _viewModel.LoadAsync(3);

        _viewModel.Pagination.CurrentPage.Should().Be(3);
        _viewModel.Pagination.TotalCount.Should().Be(30);
        _viewModel.Pagination.TotalPages.Should().Be(3); // 30 items / page size 12, rounded up
    }

    [Fact]
    public async Task LoadAsync_ReplacesPreviousProducts()
    {
        SetupProducts(page: 1, pantryOnly: false, totalCount: 2, CreateProduct(1), CreateProduct(2));
        await _viewModel.LoadAsync(1);
        SetupProducts(page: 2, pantryOnly: false, totalCount: 13, CreateProduct(3));

        await _viewModel.LoadAsync(2);

        _viewModel.Products.Select(p => p.Id).Should().Equal(3);
    }

    [Fact]
    public async Task LoadAsync_WhenDataIsEmpty_ClearsProductsAndResetsTotals()
    {
        SetupProducts(page: 1, pantryOnly: false, totalCount: 2, CreateProduct(1), CreateProduct(2));
        await _viewModel.LoadAsync(1);
        SetupProducts(page: 1, pantryOnly: false, totalCount: 0);

        await _viewModel.LoadAsync(1);

        _viewModel.Products.Should().BeEmpty();
        _viewModel.Pagination.TotalCount.Should().Be(0);
        _viewModel.Pagination.TotalPages.Should().Be(0);
    }

    [Fact]
    public async Task LoadAsync_WhenDataIsNull_ClearsProducts()
    {
        SetupProducts(page: 1, pantryOnly: false, totalCount: 1, CreateProduct(1));
        await _viewModel.LoadAsync(1);
        _api.Setup(a => a.GetProductsAsync(1, It.IsAny<int>(), false))
            .ReturnsAsync(new ApiResponse<IEnumerable<ProductDto>> { Data = null, TotalCount = 0 });

        await _viewModel.LoadAsync(1);

        _viewModel.Products.Should().BeEmpty();
    }

    [Fact]
    public async Task LoadAsync_WhenResponseIsNull_KeepsPreviousProductsAndPagination()
    {
        SetupProducts(page: 1, pantryOnly: false, totalCount: 1, CreateProduct(1));
        await _viewModel.LoadAsync(1);
        _api.Setup(a => a.GetProductsAsync(2, It.IsAny<int>(), false))
            .ReturnsAsync((ApiResponse<IEnumerable<ProductDto>>?)null);

        await _viewModel.LoadAsync(2);

        _viewModel.Products.Select(p => p.Id).Should().Equal(1);
        _viewModel.Pagination.CurrentPage.Should().Be(1);
        _viewModel.IsLoading.Should().BeFalse();
    }

    [Fact]
    public async Task LoadAsync_RefreshesActivePantryProductFromLoadedData()
    {
        _pantry.Open(CreateProduct(1, name: "Old"));
        SetupProducts(page: 1, pantryOnly: false, totalCount: 1, CreateProduct(1, name: "New"));

        await _viewModel.LoadAsync(1);

        _pantry.ActiveProduct.Should().NotBeNull();
        _pantry.ActiveProduct!.ProductName.Should().Be("New");
    }

    [Fact]
    public async Task LoadAsync_WhenActivePantryProductIsNotInLoadedData_ClosesPantry()
    {
        _pantry.Open(CreateProduct(1));
        SetupProducts(page: 1, pantryOnly: false, totalCount: 1, CreateProduct(2));

        await _viewModel.LoadAsync(1);

        _pantry.ActiveProduct.Should().BeNull();
    }

    private async Task RaisePublisherEventAsync(string publisher)
    {
        switch (publisher)
        {
            case "pantry":
                _api.Setup(a => a.QuickAddPantryItemAsync(1))
                    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
                await _pantry.QuickAddAsync(1);
                break;
            case "form":
                _api.Setup(a => a.CreateProductAsync(It.IsAny<ProductDto>()))
                    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
                await _form.SaveAsync();
                break;
            case "deletion":
                _api.Setup(a => a.DeleteProductAsync(1))
                    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
                _deletion.Prompt(1);
                await _deletion.ExecuteAsync();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(publisher), publisher, "Unknown event publisher.");
        }
    }

    private void SetupProducts(int page, bool pantryOnly, int totalCount, params ProductDto[] products) =>
        _api.Setup(a => a.GetProductsAsync(page, It.IsAny<int>(), pantryOnly))
            .ReturnsAsync(new ApiResponse<IEnumerable<ProductDto>> { Data = products, TotalCount = totalCount });

    private static ProductDto CreateProduct(int id, string name = "Product") =>
        new() { Id = id, ProductName = name };
}

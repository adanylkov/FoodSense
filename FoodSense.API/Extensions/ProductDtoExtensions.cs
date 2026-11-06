using FoodSense.API.Data.Models;
using FoodSense.Common.Models;

namespace FoodSense.API.Extensions;

public static class ProductDtoExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Barcode = product.Barcode,
            ProductName = product.Name,
            CategoryId = product.CategoryId,
            FrontImageUrl = product.FrontImageUrl,
            Nutrients = product.Nutrients.ToNutrientsDto(),
            PantryItems = product.PantryItems.ToPantryItemDtos()
        };
    }

    /// <summary>Maps a persisted local product onto a search result (source: local catalog).</summary>
    public static ProductSearchResultDto ToSearchResult(this Product product)
    {
        return new ProductSearchResultDto
        {
            ProductId = product.Id,
            Barcode = product.Barcode,
            Name = product.Name,
            ImageUrl = string.IsNullOrWhiteSpace(product.FrontImageUrl) ? null : product.FrontImageUrl,
            Nutrients = product.Nutrients.ToNutrientsDto(),
            Source = ProductSource.Local
        };
    }

    /// <summary>Maps the product form onto the local product entity. <c>Id</c> = 0 means a new row,
    /// which EF Core inserts when the product is persisted together with its pantry item.</summary>
    public static Product ToProduct(this ProductDto dto)
    {
        return new Product
        {
            Id = dto.Id,
            Barcode = dto.Barcode,
            Name = dto.ProductName,
            FrontImageUrl = dto.FrontImageUrl ?? string.Empty,
            CategoryId = dto.CategoryId,
            Nutrients = dto.Nutrients.ToNutrients()
        };
    }

    public static IEnumerable<PantryItemDto> ToPantryItemDtos(this IEnumerable<PantryItem> pantryItems)
    {
        return pantryItems.Select(p => p.ToDto());
    }

    public static PantryItemDto ToDto(this PantryItem pantryItem)
    {
        return new PantryItemDto
        {
            Id = pantryItem.Id,
            ProductId = pantryItem.ProductId,
            Quantity = pantryItem.Quantity,
            UserId = pantryItem.UserId,
            AddedAt = pantryItem.AddedAt,
            UpdatedAt = pantryItem.UpdatedAt
        };
    }

    public static IEnumerable<ProductDto> ToProductDtos(this IEnumerable<Product> products)
    {
        return products.Select(p => p.ToDto());
    }
}

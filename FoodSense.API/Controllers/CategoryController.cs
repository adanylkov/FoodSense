using FoodSense.API.Data;
using FoodSense.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FoodSense.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CategoryController(
    FoodSenseDbContext context,
    IMemoryCache cache) : ControllerBase
{
    private const string CategoriesCacheKey = "categories-tree";

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(
        CancellationToken cancellationToken)
    {
        var categories = await cache.GetOrCreateAsync(
            CategoriesCacheKey,
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                entry.SlidingExpiration = TimeSpan.FromMinutes(1);

                var flatCategories = await context.Categories
                    .AsNoTracking()
                    .Select(category => new CategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name,
                        ParentId = category.ParentId
                    })
                    .ToListAsync(cancellationToken);

                return BuildCategoryTree(flatCategories);
            });

        return Ok(categories ?? []);
    }

    private static List<CategoryDto> BuildCategoryTree(
        IReadOnlyCollection<CategoryDto> flatCategories)
    {
        var categoriesById = flatCategories.ToDictionary(category => category.Id);
        var roots = new List<CategoryDto>();

        foreach (var category in flatCategories)
        {
            if (category.ParentId is null)
            {
                roots.Add(category);
            }
            else if (categoriesById.TryGetValue(category.ParentId.Value, out var parent))
            {
                parent.Children.Add(category);
            }
        }

        SortCategoryTree(roots);
        return roots;
    }

    private static void SortCategoryTree(List<CategoryDto> categories)
    {
        categories.Sort((left, right) =>
            StringComparer.OrdinalIgnoreCase.Compare(left.Name, right.Name));

        foreach (var category in categories)
        {
            SortCategoryTree(category.Children);
        }
    }
}

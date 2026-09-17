using Microsoft.AspNetCore.Mvc;
using FoodSense.API.Data.Models;
using FoodSense.API.Services;
using FoodSense.Common.Models;

namespace FoodSense.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductMatchingController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductMatchingController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("match")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductMappingDto>>>> MatchProducts([FromBody] List<string> productNames)
    {
        var products = await _productService.GetProductsByMappingNamesAsync(productNames);
        var productMappings = products.Select(product => new ProductMappingDto {
            Barcode = product?.Barcode ?? string.Empty,
            ProductId = product?.Id ?? 0,
            ProductName = product?.ProductName ?? string.Empty
        }).ToArray();

        return Ok(new ApiResponse<IEnumerable<ProductMappingDto>>
        {
            Data = productMappings,
            TotalCount = productMappings.Length
        });
    }
}

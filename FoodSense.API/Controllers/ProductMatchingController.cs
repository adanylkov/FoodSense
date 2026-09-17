using Microsoft.AspNetCore.Mvc;
using FoodSense.API.Data.Models;
using FoodSense.API.Services;
using FoodSense.Common.Models;
using FoodSense.API.Repositories;

namespace FoodSense.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductMatchingController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IProductMappingRepository _productMappingRepository;

    public ProductMatchingController(IProductService productService, IProductMappingRepository productMappingRepository)
    {
        _productService = productService;
        _productMappingRepository = productMappingRepository;
    }

    [HttpPost("match")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductMappingDto>>>> MatchProducts([FromBody] List<string> productNames)
    {

        var mappings = await _productMappingRepository.GetByProductNamesAsync(productNames);
        var productMappings = productNames.Select(name =>
        {
            var mapping = mappings.FirstOrDefault(m => m.ProductName == name);
            return new ProductMappingDto
            {
                Barcode = mapping?.Product?.Barcode ?? string.Empty,
                ProductId = mapping?.Product?.Id ?? 0,
                ProductName = mapping?.Product?.ProductName ?? string.Empty,
            };
        }).ToArray();

        return Ok(new ApiResponse<IEnumerable<ProductMappingDto>>
        {
            Data = productMappings,
            TotalCount = productMappings.Length
        });
    }

    [HttpPost("mapping")]
    public async Task<IActionResult> AddProductMapping([FromBody] ProductMappingDto mappingDto)
    {
        if (mappingDto == null || string.IsNullOrWhiteSpace(mappingDto.ProductName) || mappingDto.ProductId <= 0)
        {
            return BadRequest("Invalid product mapping data.");
        }

        await _productService.AddProductMappingAsync(mappingDto.ProductName, mappingDto.ProductId);
        return Ok();
    }
}

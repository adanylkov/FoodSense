using FoodSense.API.Data.Models;
using FoodSense.API.Services;
using Microsoft.AspNetCore.Mvc;
namespace FoodSense.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FoodController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            if (page < 1)
            {
                return BadRequest("Page must be greater than 0.");
            }

            if (pageSize < 1 || pageSize > 200)
            {
                return BadRequest("Page size must be between 1 and 200.");
            }

            var products = await productService.GetProductsAsync(page, pageSize);
            var totalCount = await productService.GetTotalProductCountAsync();
            return Ok(new ApiResponse<IEnumerable<Product>>
            {
                Data = products,
                TotalCount = totalCount
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product is null) return NotFound();
            return Ok(product);
        }

        [HttpGet("{barcode:long}")]
        public async Task<IActionResult> GetProduct(long barcode)
        {
            var product = await productService.GetProductByBarcodeOrFetchAsync(barcode);
            if (product is null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Barcode))
            {
                return BadRequest("Barcode is required.");
            }

            var existing = await productService.GetProductByBarcodeOrFetchAsync(product.Barcode);
            if (existing != null)
            {
                return Conflict("A product with this barcode already exists.");
            }

            await productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromBody] Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Barcode))
            {
                return BadRequest("Barcode is required.");
            }

            var updatedProduct = await productService.UpdateProductAsync(id, product);
            if (updatedProduct == null) return NotFound();

            return Ok(updatedProduct);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await productService.DeleteProductAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
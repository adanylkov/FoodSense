using FoodSense.API.Data.Models;
using FoodSense.API.Extensions;
using FoodSense.API.Services;
using FoodSense.Common.Models;
using Microsoft.AspNetCore.Mvc;
namespace FoodSense.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] bool hasPantryItems = false)
        {
            if (page < 1)
            {
                return BadRequest("Page must be greater than 0.");
            }

            if (pageSize < 1 || pageSize > 200)
            {
                return BadRequest("Page size must be between 1 and 200.");
            }

            var products = await productService.GetProductsAsync(page, pageSize, hasPantryItems);
            var totalCount = await productService.GetTotalProductCountAsync(hasPantryItems);
            return Ok(new ApiResponse<IEnumerable<ProductDto>>
            {
                Data = products.ToProductDtos(),
                TotalCount = totalCount
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product is null) return NotFound();
            return Ok(product.ToDto());
        }

        [HttpGet("{barcode:long}")]
        public async Task<IActionResult> GetProduct(long barcode)
        {
            var product = await productService.GetProductByBarcodeAsync(barcode);
            if (product is null) return NotFound();
            return Ok(product);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetProductByName(string name)
        {
            var product = await productService.GetProductByNameAsync(name);
            if (product is null) return NotFound();
            return Ok(product.ToDto());
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetProductsByName(string name)
        {
            var products = await productService.GetProductsByNameAsync(name);
            if (products is null || !products.Any()) return NotFound("No products found.");
            return Ok(products.Select(p => p.ToDto()));
        }

        /// <summary>Searches the local catalog and the local OpenFoodFacts snapshot by product
        /// name. Results are candidates only: nothing is written to the catalog by this endpoint.</summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductSearchResultDto>>>> SearchProducts(
            [FromQuery] string name,
            [FromQuery] int limit = 10)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Ok(new ApiResponse<IEnumerable<ProductSearchResultDto>>
                {
                    Data = [],
                    TotalCount = 0
                });
            }

            var results = await productService.SearchProductsByNameAsync(name, limit);
            return Ok(new ApiResponse<IEnumerable<ProductSearchResultDto>>
            {
                Data = results,
                TotalCount = results.Count
            });
        }

        /// <summary>Persists a product candidate (for example an OpenFoodFacts hit with <c>Id</c> = 0)
        /// and returns it with its catalog id. Called only when the user explicitly adds the product
        /// (to the pantry or to a meal) — searching never writes to the catalog.</summary>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> GetOrCreateProduct([FromBody] ProductDto product)
        {
            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                return BadRequest("Product name is required.");
            }

            product.Barcode = NormalizeBarcode(product.Barcode);

            try
            {
                var persisted = await productService.GetOrCreateProductAsync(product);
                return Ok(persisted.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto product)
        {
            product.Barcode = NormalizeBarcode(product.Barcode);

            if (product.Barcode is not null)
            {
                var existing = await productService.GetProductByBarcodeAsync(product.Barcode);
                if (existing is not null && existing.Id > 0)
                {
                    return Conflict("A product with this barcode already exists.");
                }
            }

            var created = await productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] ProductDto product)
        {
            product.Barcode = NormalizeBarcode(product.Barcode);

            var updatedProduct = await productService.UpdateProductAsync(id, product);
            if (updatedProduct == null) return NotFound();

            return Ok(updatedProduct);
        }

        private static string? NormalizeBarcode(string? barcode)
            => string.IsNullOrWhiteSpace(barcode) ? null : barcode.Trim();

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await productService.DeleteProductAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}

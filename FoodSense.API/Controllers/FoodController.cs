using AutoMapper;
using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenFoodFactsCSharp.Services.Interfaces;

namespace FoodSense.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FoodController(FoodSenseDbContext context, IOpenFoodFactsWrapper foodFactsWrapper, IMapper mapper) : ControllerBase
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

            var query = context.Products.AsNoTracking().OrderBy(p => p.Id);
            var totalCount = await query.CountAsync();
            var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return Ok(new ApiResponse<IEnumerable<Product>>
            {
                Data = products,
                TotalCount = totalCount
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("{barcode:long}")]
        public async Task<IActionResult> GetProduct(long barcode)
        {
            var productResponse = await foodFactsWrapper.FetchProductByCodeAsync(barcode.ToString());
            if (productResponse.Status is not true || productResponse.Product is null)
            {
                return NotFound();
            }

            var dbProduct = await context.Products.FirstOrDefaultAsync(p => p.Barcode == productResponse.Code);
            if (dbProduct is null)
            {
                dbProduct = mapper.Map<Product>(productResponse.Product);
                dbProduct.Barcode = productResponse.Code;
                await context.Products.AddAsync(dbProduct);
            await context.SaveChangesAsync();
        }

            return Ok(dbProduct);
    }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Barcode))
            {
                return BadRequest("Barcode is required.");
}

            var barcodeExists = await context.Products.AnyAsync(p => p.Barcode == product.Barcode);
            if (barcodeExists)
            {
                return Conflict("A product with this barcode already exists.");
            }

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromBody] Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Barcode))
            {
                return BadRequest("Barcode is required.");
            }

            var barcodeExists = await context.Products.AnyAsync(p => p.Id != id && p.Barcode == product.Barcode);
            if (barcodeExists)
            {
                return Conflict("A product with this barcode already exists.");
            }

            var dbProduct = await context.Products.FindAsync(id);
            if (dbProduct is null)
            {
                return NotFound();
            }

            product.Id = id;
            context.Entry(dbProduct).CurrentValues.SetValues(product);

            await context.SaveChangesAsync();

            return Ok(dbProduct);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var dbProduct = await context.Products.FindAsync(id);
            if (dbProduct is null)
            {
                return NotFound();
            }

            context.Products.Remove(dbProduct);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}


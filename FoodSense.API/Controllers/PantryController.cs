using FoodSense.API.Data.Models;
using FoodSense.API.Extensions;
using FoodSense.API.Services;
using FoodSense.Common.Models;
using FoodSense.Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FoodSense.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PantryController(IPantryService pantryService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<PantryItem>> CreatePantryItem([FromBody] AddPantryItemRequest request)
        {
            var newItem = await pantryService.AddPantryItemAsync(request); 
            return Ok(newItem.ToDto());
        }

        /// <summary>Adds a pantry item for a product that may not be in the catalog yet. The product
        /// is persisted here, never during search or barcode lookup.</summary>
        [HttpPost]
        public async Task<ActionResult<PantryItemDto>> CreatePantryItemWithProduct([FromBody] AddPantryItemWithProductRequest request)
        {
            if (request.Product is null || string.IsNullOrWhiteSpace(request.Product.ProductName))
            {
                return BadRequest("Product name is required.");
            }

            if (request.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0.");
            }

            var newItem = await pantryService.AddPantryItemWithProductAsync(request);
            return Ok(newItem.ToDto());
        }

        [HttpPut("{pantryItemId:int}")]
        public async Task<ActionResult<PantryItem>> UpdatePantryItem([FromBody] EditPantryItemRequest request)
        {
            var updated = await pantryService.UpdatePantryItemAsync(request);
            if (updated == null) return NotFound();
            return Ok(updated.ToDto());
        }

        [HttpDelete("{pantryItemId:int}")]
        public async Task<IActionResult> DeletePantryItem(int pantryItemId)
        {
            var deleted = await pantryService.RemovePantryItemAsync(pantryItemId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
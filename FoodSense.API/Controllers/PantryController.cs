using FoodSense.API.Data.Models;
using FoodSense.API.Extensions;
using FoodSense.API.Services;
using FoodSense.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodSense.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PantryController(IPantryService pantryService) : ControllerBase
    {
        [HttpPost("{productId:int}")]
        public async Task<ActionResult<PantryItem>> AddProductToPantry(int productId)
        {
            var item = await pantryService.AddOnePantryItemAsync(productId);
            return Ok(item.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<PantryItem>> CreatePantryItem([FromBody] PantryItemDto pantryItemDto, int productId)
        {
            var pantryItem = new PantryItem
            {
                ProductId = productId,
                Quantity = pantryItemDto.Quantity,
                UserId = pantryItemDto.UserId,
                AddedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var newItem = await pantryService.AddPantryItemAsync(productId, pantryItem); 
            return Ok(newItem.ToDto());
        }

        [HttpPut("{pantryItemId:int}")]
        public async Task<ActionResult<PantryItem>> UpdatePantryItem(int pantryItemId, [FromBody] PantryItemDto pantryItemDto)
        {
            var pantryItem = new PantryItem
            {
                Id = pantryItemId,
                ProductId = pantryItemDto.ProductId,
                Quantity = pantryItemDto.Quantity,
                UserId = pantryItemDto.UserId,
                AddedAt = pantryItemDto.AddedAt,
                UpdatedAt = DateTime.UtcNow
            };

            var updated = await pantryService.UpdatePantryItemAsync(pantryItemId, pantryItem);
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

        [HttpDelete("product/{productId:int}/{pantryItemId:int}")]
        public async Task<IActionResult> RemoveProductFromPantry(int productId, int pantryItemId)
        {
            var deleted = await pantryService.RemovePantryItemAsync(productId, pantryItemId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
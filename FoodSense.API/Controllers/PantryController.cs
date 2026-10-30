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
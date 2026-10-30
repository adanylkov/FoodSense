using FoodSense.API.Data.Models;
using FoodSense.API.Services;
using FoodSense.Common.Models;
using FoodSense.Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FoodSense.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CalorieTrackerController(IFoodLogService foodLogService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> LogAndConsumeFromPantry([FromBody] AddFoodLogRequest request)
    {
        var result = await foodLogService.LogConsumptionAndReducePantryAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateConsumption([FromBody] EditFoodLogRequest request)
    {
        var result = await foodLogService.EditConsumptionAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FoodLogEntry>>> GetEntries([FromQuery] DateTime date)
    {
        var entries = await foodLogService.GetEntriesByDateAsync(date.Date);
        return Ok(entries);
    }

    [HttpGet]
    public async Task<ActionResult<UserNutritionalGoals>> GetGoals()
    {
        var goals = await foodLogService.GetUserGoalsAsync();
        return Ok(goals);
    }
}

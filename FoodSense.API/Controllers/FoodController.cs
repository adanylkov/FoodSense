using AutoMapper;
using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenFoodFactsCSharp.Services.Interfaces;

namespace FoodSense.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FoodController(FoodSenseDbContext context, IOpenFoodFactsWrapper foodFactsWrapper, IMapper mapper, IConfiguration configuration) : ControllerBase
    {
        [HttpGet("{barcode:long}")]
        public async Task<IActionResult> GetProduct(long barcode)
        {
            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await context.Database.BeginTransactionAsync();
                await context.Database.MigrateAsync();
                await transaction.CommitAsync();
            });
            var productResponse = await foodFactsWrapper.FetchProductByCodeAsync(barcode.ToString());
            if (productResponse.Status is true)
            {
                var dbProduct = await context.Products.FirstOrDefaultAsync(p => p.Barcode == productResponse.Code);
                if (dbProduct is null)
                {
                    dbProduct = mapper.Map<Product>(productResponse.Product);
                    dbProduct.Barcode = productResponse.Code;
                    await context.Products.AddAsync(dbProduct);
                }
            }

            return Ok(productResponse.Product);
        }
    }
}

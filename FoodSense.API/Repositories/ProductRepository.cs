using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodSense.API.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(int page, int pageSize, bool hasPantryItems = false);
    Task<int> GetTotalCountAsync(bool hasPantryItems = false);
    Task<Product?> GetByIdAsync(int id);
    Task<Product?> GetByBarcodeAsync(string barcode);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}

public class ProductRepository(FoodSenseDbContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync(int page, int pageSize, bool hasPantryItems = false)
    {
        var query = context.Products.AsNoTracking();

        if (hasPantryItems)
        {
            query = query.Where(p => p.PantryItems.Any(item => item.Quantity > 0));
        }

        return await query
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.PantryItems)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(bool hasPantryItems = false)
    {
        var query = context.Products.AsQueryable();

        if (hasPantryItems)
        {
            query = query.Where(p => p.PantryItems.Any(item => item.Quantity > 0));
        }

        return await query.CountAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await context.Products.Include(p => p.PantryItems).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> GetByBarcodeAsync(string barcode)
    {
        return await context.Products.Include(p => p.PantryItems).FirstOrDefaultAsync(p => p.Barcode == barcode);
    }

    public async Task AddAsync(Product product)
    {
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        var existing = await context.Products.FindAsync(product.Id);
        if (existing != null)
        {
            context.Entry(existing).CurrentValues.SetValues(product);
            existing.Nutrients = product.Nutrients;
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product != null)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }
}


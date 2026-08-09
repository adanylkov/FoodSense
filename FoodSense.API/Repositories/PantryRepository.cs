using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodSense.API.Repositories;

public class PantryRepository(FoodSenseDbContext context) : IPantryRepository
{
    public async Task<PantryItem> AddAsync(PantryItem pantryItem)
    {
        pantryItem.AddedAt = DateTime.UtcNow;
        pantryItem.UpdatedAt = DateTime.UtcNow;

        await context.PantryItems.AddAsync(pantryItem);
        await context.SaveChangesAsync();
        return pantryItem;
    }

    public async Task<PantryItem?> GetByIdAsync(int id)
    {
        return await context.PantryItems.FindAsync(id);
    }

    public async Task<bool> UpdateAsync(PantryItem pantryItem)
    {
        var existing = await context.PantryItems.FindAsync(pantryItem.Id);
        if (existing == null) return false;
        pantryItem.ProductId = existing.ProductId; // Ensure ProductId is not changed

        context.Entry(existing).CurrentValues.SetValues(pantryItem);
        existing.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await context.PantryItems.FindAsync(id);
        if (item == null) return false;

        context.PantryItems.Remove(item);
        await context.SaveChangesAsync();
        return true;
    }
}
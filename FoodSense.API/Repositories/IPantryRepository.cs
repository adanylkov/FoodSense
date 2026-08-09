using FoodSense.API.Data.Models;

namespace FoodSense.API.Repositories;

public interface IPantryRepository
{
    Task<PantryItem> AddAsync(PantryItem pantryItem);
    Task<PantryItem?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(PantryItem pantryItem);
    Task<bool> DeleteAsync(int id);
}
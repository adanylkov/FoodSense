using FoodSense.API.Data;
using FoodSense.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodSense.API.Repositories
{
    public interface IProductMappingRepository
    {
        Task<ProductMapping?> GetByProductNameAsync(string productName);
        Task<IEnumerable<ProductMapping>> GetByProductNamesAsync(IEnumerable<string> productNames);
        Task<IEnumerable<ProductMapping>> GetAllAsync();
        Task AddAsync(ProductMapping mapping);
        Task SaveChangesAsync();
    }

    public class ProductMappingRepository : IProductMappingRepository
    {
        private readonly FoodSenseDbContext _context;

        public ProductMappingRepository(FoodSenseDbContext context)
        {
            _context = context;
        }

        public async Task<ProductMapping?> GetByProductNameAsync(string productName)
        {
            return await _context.ProductMappings
                .Include(pm => pm.Product)
                .FirstOrDefaultAsync(pm => pm.ProductName == productName);
        }

        public async Task<IEnumerable<ProductMapping>> GetAllAsync()
        {
            return await _context.ProductMappings.ToListAsync();
        }

        public async Task AddAsync(ProductMapping mapping)
        {
            await _context.ProductMappings.AddAsync(mapping);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductMapping>> GetByProductNamesAsync(IEnumerable<string> productNames)
        {
            return await _context.ProductMappings
                .Include(pm => pm.Product)
                .Where(pm => productNames.Contains(pm.ProductName))
                .ToListAsync();
        }
    }
}

using FoodSense.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodSense.API.Data;

public class FoodSenseDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.Barcode).IsUnique();
            entity.OwnsOne(p => p.Nutrients);
        });
    }
}

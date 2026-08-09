using FoodSense.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodSense.API.Data;

public class FoodSenseDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<PantryItem> PantryItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.Barcode).IsUnique();
            entity.OwnsOne(p => p.Nutrients);
        });

        modelBuilder.Entity<PantryItem>(entity =>
        {
            entity.HasKey(pi => pi.Id);

            entity.Property(pi => pi.Quantity)
                .HasPrecision(18, 2);

            entity.HasOne(pi => pi.Product)
                .WithMany(p => p.PantryItems)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for faster lookups by Product and UserId (when available)
            entity.HasIndex(pi => new { pi.ProductId, pi.UserId });
        });
    }
}

using FoodSense.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodSense.API.Data;

public class FoodSenseDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<PantryItem> PantryItems { get; set; }
    public DbSet<ProductMapping> ProductMappings { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.Barcode).IsUnique();
            entity.OwnsOne(p => p.Nutrients);
            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(c => c.Name).IsUnique();

            entity.HasData(
                new Category
                {
                    Id = 1,
                    Name = "Fruits"
                },
                new Category
                {
                    Id = 2,
                    Name = "Vegetables"
                },
                new Category
                {
                    Id = 3,
                    Name = "Grains"
                },
                new Category
                {
                    Id = 4,
                    Name = "Dairy"
                },
                new Category
                {
                    Id = 5,
                    Name = "Meat and Seafood"
                },
                new Category
                {
                    Id = 6,
                    Name = "Snacks"
                },
                new Category
                {
                    Id = 7,
                    Name = "Beverages"
                },
                new Category
                {
                    Id = 8,
                    Name = "Other"
                });
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

        modelBuilder.Entity<ProductMapping>(entity =>
        {
            entity.HasIndex(pm => pm.ProductName)
                .IsUnique();

            entity.HasOne(pm => pm.Product)
                .WithMany()
                .HasForeignKey(pm => pm.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    
}
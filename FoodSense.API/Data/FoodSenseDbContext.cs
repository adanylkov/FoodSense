using FoodSense.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using OpenFoodFactsCSharp.Models;

namespace FoodSense.API.Data;

public class FoodSenseDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Models.Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Models.Product>(entityBuilder =>
        {
            entityBuilder.HasKey(p => p.Id);
            entityBuilder.HasAlternateKey(p => p.Barcode);

            entityBuilder.Ignore(p => p.EcoscoreData);
            entityBuilder.Ignore(p => p.SelectedImages);
        });

        modelBuilder.Entity<LanguagesCodes>(entityBuilder =>
        {
            entityBuilder.HasKey(l => new { l.En, l.Fr, l.Pl });
        });

        modelBuilder.Entity<NutrientLevels>(entityBuilder =>
        {
            entityBuilder.HasKey(n => new { n.Salt, n.Sugars, n.SaturatedFat, n.Fat });
        });
    }
}

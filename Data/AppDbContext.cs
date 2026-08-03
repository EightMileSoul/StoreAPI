using Microsoft.EntityFrameworkCore;
using StoreApi.Models;

namespace StoreApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(product => product.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
            .Property(product => product.Name)
            .HasMaxLength(100);

        modelBuilder.Entity<Category>()
            .Property(category => category.Name)
            .HasMaxLength(100);

        modelBuilder.Entity<Product>()
            .ToTable(table =>
                table.HasCheckConstraint(
                    "CK_Products_Price_Positive",
                    "[Price] > 0"));

        modelBuilder.Entity<Product>()
            .HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
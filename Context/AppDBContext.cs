using Microsoft.EntityFrameworkCore;
using MinimalAPI.Models;

namespace MinimalAPI.Context;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    { }

    public DbSet<Product>? Products { get; set; }
    public DbSet<Category>? Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Category>().HasKey(c => c.CategoryId);
        mb.Entity<Category>().Property(c => c.Name).HasMaxLength(100).IsRequired();
        mb.Entity<Category>().Property(c => c.Description).HasMaxLength(150).IsRequired();

        mb.Entity<Product>().HasKey(c => c.ProductId);
        mb.Entity<Product>().Property(c => c.Name).HasMaxLength(100).IsRequired();
        mb.Entity<Product>().Property(c => c.Description).HasMaxLength(150);
        mb.Entity<Product>().Property(c => c.ImageUrl).HasMaxLength(100);
        mb.Entity<Product>().Property(c => c.Price).HasPrecision(14, 2);

        mb.Entity<Product>()
            .HasOne<Category>(c => c.Category)
            .WithMany(p => p.Products)
            .HasForeignKey(c => c.CategoryId);
    }
}
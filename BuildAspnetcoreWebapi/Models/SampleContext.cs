using Microsoft.EntityFrameworkCore;

namespace BuildAspnetcoreWebapi.Models;

public class SampleContext : DbContext
{
    public SampleContext(DbContextOptions<SampleContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(a => a.Category)
            .HasForeignKey(a => a.CategoryId);

        modelBuilder.Seed();
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }
}

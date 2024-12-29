using Microsoft.EntityFrameworkCore;
using PM.CORE.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PM.INFRASTRUCTURE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Product entity configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Stock).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Create stored procedures
            modelBuilder.Entity<Product>()
                .ToTable(tb => tb.HasTrigger("UpdateProductTimestamp"));

            // Seed some initial data
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "Sample Product 1",
                    Category = "Electronics",
                    Price = 999.99m,
                    Stock = 50
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Sample Product 2",
                    Category = "Books",
                    Price = 29.99m,
                    Stock = 100
                }
            );
        }
    }
}
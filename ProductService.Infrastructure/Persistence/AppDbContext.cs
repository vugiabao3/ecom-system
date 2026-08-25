using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Design;
namespace ProductService.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name).IsRequired();

                entity.HasOne(x => x.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(x => x.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Brand)
                      .WithMany(b => b.Products)
                      .HasForeignKey(x => x.BrandId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
                entity.HasIndex(x => x.SellerId);
            });
        }
    }
}

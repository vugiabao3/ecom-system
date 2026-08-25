using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.Status).HasConversion<string>();
                entity.Property(x => x.PaymentStatus).HasConversion<string>();
                entity.Property(x => x.PaymentMethod).HasConversion<string>();
                entity.HasIndex(x => x.UserId);
                entity.HasIndex(x => x.Status);
                entity.HasIndex(x => x.PaymentStatus);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ProductName).IsRequired();
                entity.HasOne(x => x.Order)
                      .WithMany(o => o.Items)
                      .HasForeignKey(x => x.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(x => x.SellerId);
                entity.HasIndex(x => x.ProductId);
            });

            modelBuilder.Entity<OrderStatusHistory>(entity =>
            {
                entity.ToTable("OrderStatusHistories");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Status).IsRequired();
                entity.HasOne(x => x.Order)
                      .WithMany(o => o.StatusHistory)
                      .HasForeignKey(x => x.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(x => x.OrderId);
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ShippingService.Domain.Entities;
using EcomSystem.Contracts.Enums;

namespace ShippingService.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Shipment> Shipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.ToTable("Shipments");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Status).HasConversion<string>();
                entity.HasIndex(x => x.OrderId).IsUnique();
                entity.HasIndex(x => x.ShipperId);
                entity.HasIndex(x => x.Status);
            });
        }
    }
}

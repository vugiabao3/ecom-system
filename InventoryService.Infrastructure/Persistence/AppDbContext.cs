using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using InventoryService.Domain.Entities;

namespace InventoryService.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<InventoryItem> InventoryItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InventoryItem>(entity =>
            {
                entity.ToTable("InventoryItems");
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.ProductId).IsUnique();
            });
        }
    }
}

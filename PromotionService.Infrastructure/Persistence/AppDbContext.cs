using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using PromotionService.Domain.Entities;

namespace PromotionService.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<UserPoint> UserPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.ToTable("Promotions");
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Code).IsUnique();
                entity.HasIndex(x => x.SellerId);
            });

            modelBuilder.Entity<UserPoint>(entity =>
            {
                entity.ToTable("UserPoints");
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId).IsUnique();
            });
        }
    }
}

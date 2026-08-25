using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDeleted);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Email).IsRequired();
                entity.Property(x => x.FullName).IsRequired();
                entity.HasIndex(x => x.Email).IsUnique();
                entity.HasIndex(x => x.Role);
                entity.Property(x => x.CurrentAddress);
                entity.Property(x => x.CurrentLocation);
            });

            modelBuilder.Entity<UserActivityLog>(entity =>
            {
                entity.ToTable("UserActivityLogs");
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId);
            });

            modelBuilder.Entity<UserAddress>(entity =>
            {
                entity.ToTable("UserAddresses");
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId);
            });

            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.ToTable("UserSessions");
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId);
            });
        }

        public DbSet<UserActivityLog> UserActivityLogs { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
    }
}

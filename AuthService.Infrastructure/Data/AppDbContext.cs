using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuthUser> AuthUsers { get; set; }

        public DbSet<UserSession> UserSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthUser>(entity =>
            {
                entity.ToTable("AuthUsers");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Email).IsRequired();
                entity.HasIndex(x => x.Email).IsUnique();
            });

            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.ToTable("UserSessions");
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId);
            });
        }
    }
}

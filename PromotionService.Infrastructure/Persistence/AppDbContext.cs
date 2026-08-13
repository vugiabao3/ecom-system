using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PromotionService.Domain.Entities;

using System.Threading.Tasks;

namespace PromotionService.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<UserPoint> UserPoints { get; set; }

    }
}

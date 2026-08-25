using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PromotionService.Infrastructure.Persistence;

namespace PromotionService.Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=PromotionServiceDb;User Id=sa;Password=Vgb@24062006;TrustServerCertificate=True");

        return new AppDbContext(optionsBuilder.Options);
    }
}

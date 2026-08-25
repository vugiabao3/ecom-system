using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using InventoryService.Infrastructure.Persistence;

namespace InventoryService.Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=InventoryServiceDb;User Id=sa;Password=Vgb@24062006;TrustServerCertificate=True");

        return new AppDbContext(optionsBuilder.Options);
    }
}

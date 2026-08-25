using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProductService.Infrastructure.Persistence;

namespace ProductService.Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=ProductServiceDb;User Id=sa;Password=Vgb@24062006;TrustServerCertificate=True");

        return new AppDbContext(optionsBuilder.Options);
    }
}

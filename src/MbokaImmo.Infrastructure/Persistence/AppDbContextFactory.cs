using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Persistence;

namespace MBOKA_IMMO;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=mboka_immo;Username=postgres;Password=123456"
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}
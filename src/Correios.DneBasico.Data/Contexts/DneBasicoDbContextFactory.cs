using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Correios.DneBasico.Data.Contexts;

public class DneBasicoDbContextFactory : IDesignTimeDbContextFactory<DneBasicoDbContext>
{
    public DneBasicoDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DneBasicoDbContext>();

        var connectionString = configuration.GetConnectionString("eDNE");

        optionsBuilder.UseNpgsql(connectionString);

        return new DneBasicoDbContext(optionsBuilder.Options);
    }
}
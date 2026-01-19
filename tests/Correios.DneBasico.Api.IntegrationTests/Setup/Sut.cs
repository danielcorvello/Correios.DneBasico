using Correios.DneBasico.Data.Contexts;
using Correios.DneBasico.Importer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Correios.DneBasico.Api.IntegrationTests.Setup;

public class Sut : AppFixture<Program>
{
    private PostgreSqlContainer? postgreSqlContainer;

    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    protected override async ValueTask PreSetupAsync()
    {
        postgreSqlContainer = new PostgreSqlBuilder("postgres:16")
            .WithDatabase("dnebasico_tests")
            .WithUsername("db")
            .WithPassword("db")
            .Build();

        await postgreSqlContainer.StartAsync();

        var connectionString = postgreSqlContainer.GetConnectionString();

        Environment.SetEnvironmentVariable("ConnectionStrings__eDne", connectionString);

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContext<DneBasicoDbContext>(options =>
            options.UseNpgsql(connectionString));

        using var serviceProvider = serviceCollection.BuildServiceProvider();
        using var dbContext = serviceProvider.GetRequiredService<DneBasicoDbContext>();
        await dbContext.Database.MigrateAsync();

        var edneImporter = new EdneImporter(serviceProvider);
        await edneImporter.ImportarTudoAsync();
    }
}

public class SutCollection : TestCollection<Sut>;
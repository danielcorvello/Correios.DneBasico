using Correios.DneBasico.Data.Contexts;
using Correios.DneBasico.Importer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var runOnStart = configuration["RunOnStart"];
if (string.IsNullOrEmpty(runOnStart) || runOnStart?.ToLower() != "true")
{
    Console.WriteLine("O importador está configurado para não ser executado automaticamente.");
    Console.WriteLine("Para executar automaticamente, defina a chave 'RunOnStart' como 'true' no arquivo appsettings.json.");
    Console.WriteLine("Pressione Enter para sair...");
    Console.ReadLine();
    return;
}

var serviceProvider = new ServiceCollection()
    .AddDbContext<DneBasicoDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("eDNE")))
    .BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DneBasicoDbContext>();

    // Se precisar recriar o banco de dados, descomente a linha abaixo
    // CUIDADO! Apaga a base e todos os dados
    dbContext.Database.EnsureDeleted();

    // Aplica as migrations
    dbContext.Database.Migrate();

    Console.WriteLine("Database inicializado e migrations aplicadas com sucesso!");
}

var watch = Stopwatch.StartNew();

var edne = new EdneImporter(serviceProvider);
await edne.ImportarTudoAsync();

watch.Stop();

TimeSpan t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);
string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);

Console.WriteLine($"Tempo de execução total: {answer}");

Console.WriteLine("Importação concluída");

Thread.Sleep(10000);

// Fim do programa
Environment.Exit(0);
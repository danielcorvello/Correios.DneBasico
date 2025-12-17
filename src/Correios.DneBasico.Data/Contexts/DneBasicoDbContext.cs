namespace Correios.DneBasico.Data.Contexts;

public class DneBasicoDbContext : DbContext
{
    public DneBasicoDbContext(DbContextOptions<DneBasicoDbContext> options) : base(options)
    {
    }

    public DbSet<Cep> Ceps { get; set; } = default!;
    public DbSet<Bairro> Bairros { get; set; } = default!;
    public DbSet<CaixaPostalComunitaria> CaixasPostaisComunitarias { get; set; } = default!;
    public DbSet<Estado> Estados { get; set; } = default!;
    public DbSet<FaixaCaixaPostalComunitaria> FaixasCaixaPostalComunitaria { get; set; } = default!;
    public DbSet<FaixaCaixaPostalUop> FaixasCaixaPostalUop { get; set; } = default!;
    public DbSet<FaixaCepBairro> FaixasCepBairro { get; set; } = default!;
    public DbSet<FaixaCepEstado> FaixasCepEstado { get; set; } = default!;
    public DbSet<FaixaCepLocalidade> FaixasCepLocalidade { get; set; } = default!;
    public DbSet<FaixaNumericaSeccionamento> FaixasNumericasSeccionamento { get; set; } = default!;
    public DbSet<GrandeUsuario> GrandesUsuarios { get; set; } = default!;
    public DbSet<Localidade> Localidades { get; set; } = default!;
    public DbSet<Logradouro> Logradouros { get; set; } = default!;
    public DbSet<Pais> Paises { get; set; } = default!;
    public DbSet<UnidadeOperacional> UnidadesOperacionais { get; set; } = default!;
    public DbSet<VariacaoBairro> VariacoesBairro { get; set; } = default!;
    public DbSet<VariacaoLocalidade> VariacoesLocalidade { get; set; } = default!;
    public DbSet<VariacaoLogradouro> VariacoesLogradouro { get; set; } = default!;

    // ... código existente ...
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DneBasicoDbContext).Assembly);

        List<Estado> estados =
        [
            new Estado { Uf = "RO", Nome = "Rondônia", Ibge = "11" },
            new Estado { Uf = "AC", Nome = "Acre", Ibge = "12" },
            new Estado { Uf = "AM", Nome = "Amazonas", Ibge = "13" },
            new Estado { Uf = "RR", Nome = "Roraima", Ibge = "14" },
            new Estado { Uf = "PA", Nome = "Pará", Ibge = "15" },
            new Estado { Uf = "AP", Nome = "Amapá", Ibge = "16" },
            new Estado { Uf = "TO", Nome = "Tocantins", Ibge = "17" },
            new Estado { Uf = "MA", Nome = "Maranhão", Ibge = "21" },
            new Estado { Uf = "PI", Nome = "Piauí", Ibge = "22" },
            new Estado { Uf = "CE", Nome = "Ceará", Ibge = "23" },
            new Estado { Uf = "RN", Nome = "Rio Grande do Norte", Ibge = "24" },
            new Estado { Uf = "PB", Nome = "Paraíba", Ibge = "25" },
            new Estado { Uf = "PE", Nome = "Pernambuco", Ibge = "26" },
            new Estado { Uf = "AL", Nome = "Alagoas", Ibge = "27" },
            new Estado { Uf = "SE", Nome = "Sergipe", Ibge = "28" },
            new Estado { Uf = "BA", Nome = "Bahia", Ibge = "29" },
            new Estado { Uf = "MG", Nome = "Minas Gerais", Ibge = "31" },
            new Estado { Uf = "ES", Nome = "Espírito Santo", Ibge = "32" },
            new Estado { Uf = "RJ", Nome = "Rio de Janeiro", Ibge = "33" },
            new Estado { Uf = "SP", Nome = "São Paulo", Ibge = "35" },
            new Estado { Uf = "PR", Nome = "Paraná", Ibge = "41" },
            new Estado { Uf = "SC", Nome = "Santa Catarina", Ibge = "42" },
            new Estado { Uf = "RS", Nome = "Rio Grande do Sul", Ibge = "43" },
            new Estado { Uf = "MS", Nome = "Mato Grosso do Sul", Ibge = "50" },
            new Estado { Uf = "MT", Nome = "Mato Grosso", Ibge = "51" },
            new Estado { Uf = "GO", Nome = "Goiás", Ibge = "52" },
            new Estado { Uf = "DF", Nome = "Distrito Federal", Ibge = "53" }
        ];

        modelBuilder.Entity<Estado>().HasData(estados);
    }
}
namespace Correios.DneBasico.Data.Contexts;

public class DneBasicoDbContext : DbContext
{
    public DneBasicoDbContext(DbContextOptions<DneBasicoDbContext> options) : base(options)
    {
    }

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DneBasicoDbContext).Assembly);
    }
}
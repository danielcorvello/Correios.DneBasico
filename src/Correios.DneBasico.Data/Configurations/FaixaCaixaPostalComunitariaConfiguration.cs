namespace Correios.DneBasico.Data.Configurations;

public class FaixaCaixaPostalComunitariaConfiguration : IEntityTypeConfiguration<FaixaCaixaPostalComunitaria>
{
    public void Configure(EntityTypeBuilder<FaixaCaixaPostalComunitaria> builder)
    {
        builder.ToTable("faixas_caixa_postal_comunitaria");

        builder.HasKey(f => new { f.CaixaPostalComunitariaId, f.CaixaPostalInicial });

        builder.Property(f => f.CaixaPostalComunitariaId)
            .HasColumnName("cpc_nu")
            .ValueGeneratedNever();

        builder.Property(f => f.CaixaPostalInicial)
            .HasColumnName("cpc_inicial")
            .IsRequired()
            .HasMaxLength(6);

        builder.Property(f => f.CaixaPostalFinal)
            .HasColumnName("cpc_final")
            .IsRequired()
            .HasMaxLength(6);
    }
}
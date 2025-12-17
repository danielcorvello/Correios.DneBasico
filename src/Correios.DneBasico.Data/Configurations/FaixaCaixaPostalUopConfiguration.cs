namespace Correios.DneBasico.Data.Configurations;

public class FaixaCaixaPostalUopConfiguration : IEntityTypeConfiguration<FaixaCaixaPostalUop>
{
    public void Configure(EntityTypeBuilder<FaixaCaixaPostalUop> builder)
    {
        builder.ToTable("faixas_caixa_postal_uop");

        builder.HasKey(f => new { f.UnidadeOperacionalId, f.CaixaPostalInicial });

        builder.Property(f => f.UnidadeOperacionalId)
            .HasColumnName("uop_nu")
            .ValueGeneratedNever();

        builder.Property(f => f.CaixaPostalInicial)
            .HasColumnName("fnc_inicial")
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(f => f.CaixaPostalFinal)
            .HasColumnName("fnc_final")
            .IsRequired()
            .HasMaxLength(8);
    }
}
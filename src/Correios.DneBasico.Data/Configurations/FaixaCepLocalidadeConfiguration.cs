namespace Correios.DneBasico.Data.Configurations;

public class FaixaCepLocalidadeConfiguration : IEntityTypeConfiguration<FaixaCepLocalidade>
{
    public void Configure(EntityTypeBuilder<FaixaCepLocalidade> builder)
    {
        builder.ToTable("faixas_cep_localidade");

        builder.HasKey(f => new { f.LocalidadeId, f.CepInicial, f.TipoFaixa });

        builder.Property(f => f.LocalidadeId)
            .HasColumnName("loc_nu")
            .ValueGeneratedNever();

        builder.Property(f => f.CepInicial)
            .HasColumnName("loc_cep_ini")
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(f => f.CepFinal)
            .HasColumnName("loc_cep_fim")
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(f => f.TipoFaixa)
            .HasColumnName("loc_tipo_faixa")
            .IsRequired();
    }
}
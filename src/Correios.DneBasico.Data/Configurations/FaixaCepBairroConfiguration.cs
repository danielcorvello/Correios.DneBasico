namespace Correios.DneBasico.Data.Configurations;

public class FaixaCepBairroConfiguration : IEntityTypeConfiguration<FaixaCepBairro>
{
    public void Configure(EntityTypeBuilder<FaixaCepBairro> builder)
    {
        builder.ToTable("faixas_cep_bairro");

        builder.HasKey(b => new { b.BairroId, b.CepInicial });

        builder.Property(b => b.BairroId)
            .HasColumnName("bai_nu")
            .ValueGeneratedNever();

        builder.Property(b => b.CepInicial)
            .HasColumnName("fcb_cep_ini")
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(b => b.CepFinal)
            .HasColumnName("fcb_cep_fim")
            .IsRequired()
            .HasMaxLength(8);
    }
}
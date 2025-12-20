namespace Correios.DneBasico.Data.Configurations;

public class FaixaNumericaSeccionamentoConfiguration : IEntityTypeConfiguration<FaixaNumericaSeccionamento>
{
    public void Configure(EntityTypeBuilder<FaixaNumericaSeccionamento> builder)
    {
        builder.ToTable("faixas_numericas_seccionamento");

        builder.HasKey(x => new { x.LogradouroId, x.SeccionamentoInicial, x.ParidadeLado });

        builder.Property(x => x.LogradouroId)
            .HasColumnName("log_nu")
            .ValueGeneratedNever()
            .IsRequired();


        builder.Property(x => x.SeccionamentoInicial)
            .HasColumnName("sec_nu_ini")
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.SeccionamentoFinal)
            .HasColumnName("sec_nu_fim")
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.ParidadeLado)
            .HasColumnName("sec_in_lado")
            .IsRequired()
            .HasMaxLength(1);
    }
}
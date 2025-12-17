namespace Correios.DneBasico.Data.Configurations;

public class VariacaoLogradouroConfiguration : IEntityTypeConfiguration<VariacaoLogradouro>
{
    public void Configure(EntityTypeBuilder<VariacaoLogradouro> builder)
    {
        builder.ToTable("variacoes_logradouro");

        builder.HasKey(b => new { b.LogradouroId, b.Ordem });

        builder.Property(b => b.LogradouroId)
            .HasColumnName("log_nu")
            .ValueGeneratedNever();

        builder.Property(b => b.Ordem)
            .HasColumnName("vlo_nu")
            .ValueGeneratedNever();

        builder.Property(b => b.Tipo)
            .HasColumnName("tlo_tx")
            .IsRequired()
            .HasMaxLength(36);

        builder.Property(b => b.Denominacao)
            .HasColumnName("vlo_tx")
            .IsRequired()
            .HasMaxLength(150);
    }
}
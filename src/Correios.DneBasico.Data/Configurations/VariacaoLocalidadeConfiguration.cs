namespace Correios.DneBasico.Data.Configurations;

public class VariacaoLocalidadeConfiguration : IEntityTypeConfiguration<VariacaoLocalidade>
{
    public void Configure(EntityTypeBuilder<VariacaoLocalidade> builder)
    {
        builder.ToTable("variacoes_localidade");

        builder.HasKey(b => new { b.LocalidadeId, b.Ordem });

        builder.Property(b => b.LocalidadeId)
            .HasColumnName("loc_nu")
            .ValueGeneratedNever();

        builder.Property(b => b.Ordem)
            .HasColumnName("val_nu")
            .ValueGeneratedNever();

        builder.Property(b => b.Denominacao)
            .HasColumnName("val_tx")
            .IsRequired()
            .HasMaxLength(72);
    }
}
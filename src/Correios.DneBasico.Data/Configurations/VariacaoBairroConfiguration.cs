namespace Correios.DneBasico.Data.Configurations;

public class VariacaoBairroConfiguration : IEntityTypeConfiguration<VariacaoBairro>
{
    public void Configure(EntityTypeBuilder<VariacaoBairro> builder)
    {
        builder.ToTable("variacoes_bairro");

        builder.HasKey(b => new { b.BairroId, b.Ordem });

        builder.Property(b => b.BairroId)
            .HasColumnName("bai_nu")
            .ValueGeneratedNever();

        builder.Property(b => b.Ordem)
            .HasColumnName("vdb_nu")
            .ValueGeneratedNever();

        builder.Property(b => b.Denominacao)
            .HasColumnName("vdb_tx")
            .IsRequired()
            .HasMaxLength(72);
    }
}
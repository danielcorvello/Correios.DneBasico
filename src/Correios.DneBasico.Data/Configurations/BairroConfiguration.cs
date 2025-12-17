namespace Correios.DneBasico.Data.Configurations;

public class BairroConfiguration : IEntityTypeConfiguration<Bairro>
{
    public void Configure(EntityTypeBuilder<Bairro> builder)
    {
        builder.ToTable("bairros");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("bai_nu")
            .ValueGeneratedNever();

        builder.Property(b => b.Uf)
            .HasColumnName("ufe_sg")
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(b => b.LocalidadeId)
            .HasColumnName("loc_nu")
            .IsRequired();

        builder.Property(b => b.Nome)
            .HasColumnName("bai_no")
            .IsRequired()
            .HasMaxLength(72);

        builder.Property(b => b.NomeAbreviado)
            .HasColumnName("bai_no_abrev")
            .HasMaxLength(36);
    }
}
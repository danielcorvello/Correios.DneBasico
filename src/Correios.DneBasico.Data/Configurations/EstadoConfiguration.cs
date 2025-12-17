namespace Correios.DneBasico.Data.Configurations;

public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
{
    public void Configure(EntityTypeBuilder<Estado> builder)
    {
        builder.ToTable("estados");

        builder.HasKey(u => u.Uf);

        builder.Property(u => u.Uf)
            .HasColumnName("ufe_sg")
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(u => u.Nome)
            .HasColumnName("ufe_no")
            .IsRequired()
            .HasMaxLength(19);

        builder.Property(u => u.Ibge)
            .HasColumnName("ufe_nu")
            .IsRequired()
            .HasMaxLength(2);
    }
}
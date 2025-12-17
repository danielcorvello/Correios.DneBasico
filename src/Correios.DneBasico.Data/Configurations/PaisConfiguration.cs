namespace Correios.DneBasico.Data.Configurations;

public class PaisConfiguration : IEntityTypeConfiguration<Pais>
{
    public void Configure(EntityTypeBuilder<Pais> builder)
    {
        builder.ToTable("paises");

        builder.HasKey(c => c.Sigla);

        builder.Property(c => c.Sigla)
            .HasColumnName("pai_sg")
            .ValueGeneratedNever();

        builder.Property(c => c.SiglaAlternativa)
            .HasColumnName("pai_sg_alternativa");

        builder.Property(c => c.NomePortugues)
            .HasColumnName("pai_no_portugues");

        builder.Property(c => c.NomeIngles)
            .HasColumnName("pai_no_ingles");

        builder.Property(c => c.NomeFrances)
            .HasColumnName("pai_no_frances");

        builder.Property(c => c.Abreviatura)
            .HasColumnName("pai_abreviatura");
    }
}
namespace Correios.DneBasico.Data.Configurations;

public class FaixaCepEstadoConfiguration : IEntityTypeConfiguration<FaixaCepEstado>
{
    public void Configure(EntityTypeBuilder<FaixaCepEstado> builder)
    {
        builder.ToTable("faixas_cep_estado");

        builder.HasKey(f => new { f.Uf, f.CepInicial });

        builder.Property(f => f.Uf)
            .HasColumnName("ufe_sg")
            .IsRequired()
            .HasMaxLength(2);

        builder.HasOne(l => l.Estado)
            .WithMany(l => l.Faixas)
            .HasForeignKey(l => l.Uf)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(f => f.CepInicial)
            .HasColumnName("ufe_cep_ini")
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(f => f.CepFinal)
            .HasColumnName("ufe_cep_fim")
            .IsRequired()
            .HasMaxLength(8);
    }
}

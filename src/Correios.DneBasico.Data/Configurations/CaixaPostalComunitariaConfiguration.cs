namespace Correios.DneBasico.Data.Configurations;

public class CaixaPostalComunitariaConfiguration : IEntityTypeConfiguration<CaixaPostalComunitaria>
{
    public void Configure(EntityTypeBuilder<CaixaPostalComunitaria> builder)
    {
        builder.ToTable("caixas_postais_comunitarias");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("cpc_nu")
            .ValueGeneratedNever();

        builder.Property(c => c.Uf)
            .HasColumnName("ufe_sg")
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(c => c.LocalidadeId)
            .HasColumnName("loc_nu")
            .IsRequired();

        builder.Property(c => c.Nome)
            .HasColumnName("cpc_no")
            .IsRequired()
            .HasMaxLength(72);

        builder.Property(c => c.Endereco)
            .HasColumnName("cpc_endereco")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Cep)
            .HasColumnName("cep")
            .IsRequired()
            .HasMaxLength(8);
    }
}
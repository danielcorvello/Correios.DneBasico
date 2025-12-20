namespace Correios.DneBasico.Data.Configurations;

public class LocalidadeConfiguration : IEntityTypeConfiguration<Localidade>
{
    public void Configure(EntityTypeBuilder<Localidade> builder)
    {
        builder.ToTable("localidades");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("loc_nu")
            .ValueGeneratedNever();

        builder.Property(l => l.Uf)
            .HasColumnName("ufe_sg")
            .IsRequired()
            .HasMaxLength(2);

        builder.HasOne(l => l.Estado)
            .WithMany()
            .HasForeignKey(l => l.Uf)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(l => l.Nome)
            .HasColumnName("loc_no")
            .IsRequired()
            .HasMaxLength(72);

        builder.Property(l => l.Cep)
            .HasColumnName("loc_cep")
            .HasMaxLength(8);

        builder.Property(l => l.Situacao)
            .HasColumnName("loc_in_sit");

        builder.Property(l => l.Tipo)
            .HasColumnName("loc_in_tipo_loc");

        builder.Property(l => l.SubordinacaoId)
            .HasColumnName("loc_nu_sub");

        builder.Property(l => l.NomeAbreviado)
            .HasColumnName("loc_no_abrev")
            .HasMaxLength(36);

        builder.Property(l => l.Ibge)
            .HasColumnName("mun_nu")
            .HasMaxLength(7);
    }
}
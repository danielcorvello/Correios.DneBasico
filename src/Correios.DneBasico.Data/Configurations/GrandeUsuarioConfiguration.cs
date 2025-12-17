namespace Correios.DneBasico.Data.Configurations;

public class GrandeUsuarioConfiguration : IEntityTypeConfiguration<GrandeUsuario>
{
    public void Configure(EntityTypeBuilder<GrandeUsuario> builder)
    {
        builder.ToTable("grandes_usuarios");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasColumnName("gru_nu")
            .ValueGeneratedNever();

        builder.Property(g => g.Uf)
            .HasColumnName("ufe_sg")
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(g => g.LocalidadeId)
            .HasColumnName("loc_nu")
            .IsRequired();

        builder.Property(g => g.BairroId)
            .HasColumnName("bai_nu")
            .IsRequired();

        builder.Property(g => g.LogradouroId)
            .HasColumnName("log_nu");

        builder.Property(g => g.Nome)
            .HasColumnName("gru_no")
            .IsRequired()
            .HasMaxLength(72);

        builder.Property(g => g.Endereco)
            .HasColumnName("gru_endereco")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.Cep)
            .HasColumnName("cep")
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(g => g.NomeAbreviado)
            .HasColumnName("gru_no_abrev")
            .HasMaxLength(36);
    }
}
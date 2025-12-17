namespace Correios.DneBasico.Data.Configurations;

public class LogradouroConfiguration : IEntityTypeConfiguration<Logradouro>
{
    public void Configure(EntityTypeBuilder<Logradouro> builder)
    {
        builder.ToTable("logradouros");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("log_nu")
            .ValueGeneratedNever();

        builder.Property(l => l.Uf)
            .HasColumnName("ufe_sg")
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(l => l.LocalidadeId)
            .HasColumnName("loc_nu")
            .IsRequired();

        builder.Property(l => l.BairroId)
            .HasColumnName("bai_nu_ini")
            .IsRequired();

        builder.Property(l => l.Nome)
            .HasColumnName("log_no")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.Complemento)
            .HasColumnName("log_complemento")
            .HasMaxLength(100);

        builder.Property(l => l.Cep)
            .HasColumnName("cep")
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(l => l.Tipo)
            .HasColumnName("tlo_tx")
            .IsRequired()
            .HasMaxLength(36);

        builder.Property(l => l.StatusTipo)
            .HasColumnName("log_sta_tlo")
            .HasMaxLength(1);

        builder.Property(l => l.NomeAbreviado)
            .HasColumnName("log_no_abrev")
            .HasMaxLength(36);
    }
}
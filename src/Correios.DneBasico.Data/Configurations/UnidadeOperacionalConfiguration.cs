namespace Correios.DneBasico.Data.Configurations;

public class UnidadeOperacionalConfiguration : IEntityTypeConfiguration<UnidadeOperacional>
{
    public void Configure(EntityTypeBuilder<UnidadeOperacional> builder)
    {
        builder.ToTable("unidades_operacionais");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("uop_nu")
            .IsRequired();

        builder.Property(x => x.Uf)
            .HasColumnName("ufe_sg")
            .IsRequired().HasMaxLength(2);

        builder.Property(x => x.LocalidadeId)
            .HasColumnName("loc_nu")
            .IsRequired();

        builder.Property(x => x.BairroId)
            .HasColumnName("bai_nu")
            .IsRequired();

        builder.Property(x => x.LogradouroId)
            .HasColumnName("log_nu");

        builder.Property(x => x.Nome)
            .HasColumnName("uop_no")
            .IsRequired().HasMaxLength(100);

        builder.Property(x => x.Endereco)
            .HasColumnName("uop_endereco")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Cep)
            .HasColumnName("cep")
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(x => x.CaixasPostais)
            .HasColumnName("uop_in_cp")
            .IsRequired()
            .HasMaxLength(1);

        builder.Property(x => x.NomeAbreviado)
            .HasColumnName("uop_no_abrev")
            .HasMaxLength(36);

        builder.HasOne(x => x.Estado)
            .WithMany()
            .HasForeignKey(x => x.Uf)
            .HasPrincipalKey(x => x.Uf);
    }
}
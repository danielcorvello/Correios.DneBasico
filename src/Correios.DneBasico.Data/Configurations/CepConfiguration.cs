namespace Correios.DneBasico.Data.Configurations;

public class CepConfiguration : IEntityTypeConfiguration<Cep>
{
    public void Configure(EntityTypeBuilder<Cep> builder)
    {
        builder.ToTable("ceps");

        builder.HasKey(c => c.Codigo);

        builder.Property(c => c.Codigo)
            .HasColumnName("codigo")
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(c => c.Ibge)
            .IsRequired()
            .HasColumnName("ibge")
            .HasMaxLength(7);

        builder.Property(c => c.Municipio)
            .HasColumnName("municipio")
            .IsRequired()
            .HasMaxLength(72);

        builder.Property(c => c.Uf)
            .HasColumnName("uf")
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(c => c.Bairro)
            .HasColumnName("bairro")
            .HasMaxLength(72);

        builder.Property(c => c.Distrito)
            .HasColumnName("distrito")
            .HasMaxLength(72);

        builder.Property(c => c.TipoLogradouro)
            .HasColumnName("tipo_logradouro")
            .HasMaxLength(36);

        builder.Property(c => c.Logradouro)
            .HasColumnName("logradouro")
            .HasMaxLength(100);

        builder.Property(c => c.LogradouroCompleto)
            .HasColumnName("logradouro_completo")
            .HasMaxLength(100);

        builder.Property(c => c.Complemento)
            .HasColumnName("complemento")
            .HasMaxLength(100);

        builder.Property(c => c.Unidade)
            .HasColumnName("unidade")
            .HasMaxLength(100);

        builder.Property(c => c.Geral)
            .HasColumnName("geral")
            .IsRequired();

        builder.Property(c => c.Tipo)
            .HasColumnName("tipo")
            .IsRequired();

        builder.Property(c => c.Lat)
            .HasColumnName("lat")
            .HasPrecision(10, 8);

        builder.Property(c => c.Lng)
            .HasColumnName("lng")
            .HasPrecision(11, 8);
    }
}
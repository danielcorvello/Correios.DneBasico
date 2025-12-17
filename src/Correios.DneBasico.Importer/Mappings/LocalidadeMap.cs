namespace Correios.DneBasico.Importer.Mappings;

public class LocalidadeMap : ClassMap<Localidade>
{
    public LocalidadeMap()
    {
        Map(m => m.Id).Index(0);
        Map(m => m.Uf).Index(1);
        Map(m => m.Nome).Index(2);
        Map(m => m.Cep)
            .TypeConverterOption
            .NullValues(string.Empty)
            .Index(3);
        Map(m => m.Situacao).Index(4);
        Map(m => m.Tipo)
            .TypeConverter<TipoLocalidadeConverter>()
            .Index(5);
        Map(m => m.SubordinadaId).Index(6);
        Map(m => m.NomeAbreviado).Index(7);
        Map(m => m.Ibge)
            .TypeConverterOption
            .NullValues(string.Empty)
            .Index(8);
    }
}

public class TipoLocalidadeConverter : ITypeConverter
{
    public object? ConvertFromString(
        string? text,
        IReaderRow row,
        MemberMapData memberMapData)
    {
        return text switch
        {
            "D" => TipoLocalidade.DISTRITO,
            "P" => TipoLocalidade.POVOADO,
            "M" => TipoLocalidade.MUNICIPIO,
            _ => throw new InvalidOperationException($"Tipo de localidade desconhecido: {text}")
        };
    }
    public string? ConvertToString(
        object? value,
        IWriterRow row,
        MemberMapData memberMapData)
    {
        throw new NotImplementedException();
    }
}
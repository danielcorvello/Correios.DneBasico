namespace Correios.DneBasico.Importer.Mappings;

public class FaixaCepLocalidadeMap : ClassMap<FaixaCepLocalidade>
{
    public FaixaCepLocalidadeMap()
    {
        Map(m => m.LocalidadeId).Index(0);
        Map(m => m.CepInicial).Index(1);
        Map(m => m.CepFinal).Index(2);
        Map(m => m.TipoFaixa)
            .TypeConverter<TipoFaixaConverter>()
            .Index(3);
    }
}

public class TipoFaixaConverter : ITypeConverter
{
    public object? ConvertFromString(
        string? text,
        IReaderRow row,
        MemberMapData memberMapData)
    {
        return text switch
        {
            "T" => TipoFaixaCep.TOTAL_DO_MUNICIPIO,
            "C" => TipoFaixaCep.EXCLUSIVA_SEDE_URBANA,
            _ => throw new InvalidOperationException($"Tipo de faixa desconhecido: {text}")
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

namespace Correios.DneBasico.Importer.Mappings;

public class FaixaNumericaSeccionamentoMap : ClassMap<FaixaNumericaSeccionamento>
{
    public FaixaNumericaSeccionamentoMap()
    {
        Map(m => m.LogradouroId).Index(0);
        Map(m => m.SeccionamentoInicial).Index(1);
        Map(m => m.SeccionamentoFinal).Index(2);
        Map(m => m.ParidadeLado)
            .TypeConverter<ParidadeLadoSeccionamentoConverter>()
            .Index(3);
    }
}

public class ParidadeLadoSeccionamentoConverter : ITypeConverter
{
    public object? ConvertFromString(
        string? text,
        IReaderRow row,
        MemberMapData memberMapData)
    {
        return text switch
        {
            "A" => ParidadeLadoSeccionamento.AMBOS,
            "P" => ParidadeLadoSeccionamento.PAR,
            "I" => ParidadeLadoSeccionamento.IMPAR,
            "D" => ParidadeLadoSeccionamento.DIREITO,
            "E" => ParidadeLadoSeccionamento.ESQUERDO,
            _ => throw new InvalidOperationException($"Paridade de lado desconhecida: {text}")
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


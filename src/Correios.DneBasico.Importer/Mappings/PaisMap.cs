namespace Correios.DneBasico.Importer.Mappings;

public class PaisMap : ClassMap<Pais>
{
    public PaisMap()
    {
        Map(m => m.Sigla).Index(0);
        Map(m => m.SiglaAlternativa).Index(1);
        Map(m => m.NomePortugues).Index(2);
        Map(m => m.NomeIngles).Index(3);
        Map(m => m.NomeFrances).Index(4);
        Map(m => m.Abreviatura)
            .TypeConverterOption
            .NullValues(string.Empty)
            .Index(5);
    }
}
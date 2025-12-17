namespace Correios.DneBasico.Importer.Mappings;

public class BairroMap : ClassMap<Bairro>
{
    public BairroMap()
    {
        Map(m => m.Id).Index(0);
        Map(m => m.Uf).Index(1);
        Map(m => m.LocalidadeId).Index(2);
        Map(m => m.Nome).Index(3);
        Map(m => m.NomeAbreviado)
            .TypeConverterOption
            .NullValues(string.Empty)
            .Index(4);
    }
}

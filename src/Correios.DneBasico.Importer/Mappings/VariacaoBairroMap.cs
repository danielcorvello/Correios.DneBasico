namespace Correios.DneBasico.Importer.Mappings;

public class VariacaoBairroMap : ClassMap<VariacaoBairro>
{
    public VariacaoBairroMap()
    {
        Map(m => m.BairroId).Index(0);
        Map(m => m.Ordem).Index(1);
        Map(m => m.Denominacao).Index(2);
    }
}
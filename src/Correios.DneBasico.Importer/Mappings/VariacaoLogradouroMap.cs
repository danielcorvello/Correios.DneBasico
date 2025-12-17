namespace Correios.DneBasico.Importer.Mappings;

public class VariacaoLogradouroMap : ClassMap<VariacaoLogradouro>
{
    public VariacaoLogradouroMap()
    {
        Map(m => m.LogradouroId).Index(0);
        Map(m => m.Ordem).Index(1);
        Map(m => m.Tipo).Index(2);
        Map(m => m.Denominacao).Index(3);
    }
}
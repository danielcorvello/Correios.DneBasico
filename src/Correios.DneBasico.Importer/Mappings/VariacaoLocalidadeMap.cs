
namespace Correios.DneBasico.Importer.Mappings;

public class VariacaoLocalidadeMap : ClassMap<VariacaoLocalidade>
{
    public VariacaoLocalidadeMap()
    {
        Map(m => m.LocalidadeId).Index(0);
        Map(m => m.Ordem).Index(1);
        Map(m => m.Denominacao).Index(2);
    }
}
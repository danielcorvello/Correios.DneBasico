namespace Correios.DneBasico.Importer.Mappings;

public class CaixaPostalComunitariaMap : ClassMap<CaixaPostalComunitaria>
{
    public CaixaPostalComunitariaMap()
    {
        Map(m => m.Id).Index(0);
        Map(m => m.Uf).Index(1);
        Map(m => m.LocalidadeId).Index(2);
        Map(m => m.Nome).Index(3);
        Map(m => m.Endereco).Index(4);
        Map(m => m.Cep).Index(5);
    }
}

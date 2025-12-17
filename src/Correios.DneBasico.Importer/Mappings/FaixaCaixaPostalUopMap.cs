namespace Correios.DneBasico.Importer.Mappings;

public class FaixaCaixaPostalUopMap : ClassMap<FaixaCaixaPostalUop>
{
    public FaixaCaixaPostalUopMap()
    {
        Map(m => m.UnidadeOperacionalId).Index(0);
        Map(m => m.CaixaPostalInicial).Index(1);
        Map(m => m.CaixaPostalFinal).Index(2);
    }
}

namespace Correios.DneBasico.Importer.Mappings;

public class FaixaCaixaPostalComunitariaMap : ClassMap<FaixaCaixaPostalComunitaria>
{
    public FaixaCaixaPostalComunitariaMap()
    {
        Map(m => m.CaixaPostalComunitariaId).Index(0);
        Map(m => m.CaixaPostalInicial).Index(1);
        Map(m => m.CaixaPostalFinal).Index(2);
    }
}



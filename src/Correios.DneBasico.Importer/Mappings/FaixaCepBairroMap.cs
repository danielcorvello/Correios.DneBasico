namespace Correios.DneBasico.Importer.Mappings;

public class FaixaCepBairroMap : ClassMap<FaixaCepBairro>
{
    public FaixaCepBairroMap()
    {
        Map(m => m.BairroId).Index(0);
        Map(m => m.CepInicial).Index(1);
        Map(m => m.CepFinal).Index(2);
    }
}
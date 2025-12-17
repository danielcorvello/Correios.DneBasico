namespace Correios.DneBasico.Importer.Mappings;

public class FaixaCepEstadoMap : ClassMap<FaixaCepEstado>
{
    public FaixaCepEstadoMap()
    {
        Map(m => m.Uf).Index(0);
        Map(m => m.CepInicial).Index(1);
        Map(m => m.CepFinal).Index(2);
    }
}


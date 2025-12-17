namespace Correios.DneBasico.Importer.Mappings;

public class LogradouroMap : ClassMap<Logradouro>
{
    public LogradouroMap()
    {
        Map(m => m.Id).Index(0);
        Map(m => m.Uf).Index(1);
        Map(m => m.LocalidadeId).Index(2);
        Map(m => m.BairroId).Index(3);
        // Pulamos o BAI_NU_FIM
        Map(m => m.Nome).Index(5);
        Map(m => m.Complemento)
            .TypeConverterOption
            .NullValues(string.Empty)
            .Index(6);
        Map(m => m.Cep).Index(7);
        Map(m => m.Tipo).Index(8);
        Map(m => m.StatusTipo)
            .TypeConverterOption
            .NullValues(string.Empty)
            .Index(9);
        Map(m => m.NomeAbreviado)
            .TypeConverterOption
            .NullValues(string.Empty)
            .Index(10);
    }
}
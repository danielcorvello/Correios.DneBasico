namespace Correios.DneBasico.Importer.Mappings;

public class GrandeUsuarioMap : ClassMap<GrandeUsuario>
{
    public GrandeUsuarioMap()
    {
        Map(m => m.Id).Index(0);
        Map(m => m.Uf).Index(1);
        Map(m => m.LocalidadeId).Index(2);
        Map(m => m.BairroId).Index(3);
        Map(m => m.LogradouroId).Index(4);
        Map(m => m.Nome).Index(5);
        Map(m => m.Endereco).Index(6);
        Map(m => m.Cep).Index(7);
        Map(m => m.NomeAbreviado)
            .TypeConverterOption
            .NullValues(string.Empty)
            .Index(8);
    }
}
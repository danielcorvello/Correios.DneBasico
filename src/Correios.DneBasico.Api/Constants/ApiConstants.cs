namespace Correios.DneBasico.Api.Constants;

public static class ApiConstants
{
    public static class Tags
    {
        public const string BASE = "DNE Básico";
    }

    public static class RouteNames
    {
        public const string LOCALIDADES = "localidades";
        public const string LOGRADOUROS = "logradouros";
        public const string ESTADOS = "estados";
        public const string CIDADES = "cidades";
        public const string CEPS = "ceps";
        public const string BAIRROS = "bairros";
        public const string UNIDADES_OPERACIONAIS = "unidades-operacionais";
        public const string CAIXAS_POSTAIS_COMUNITARIAS = "caixas-postais-comunitarias";
        public const string GRANDES_USUARIOS = "grandes-usuarios";
    }

    public const string FILTER_SUMMARY = "Filtro para busca. Campos permitidos: {0}";
    public const string ORDERBY_SUMMARY = "Critério de ordenação dos resultados. Campos permitidos: {0}";
    public const string GRIDYFY_INVALID_QUERY = "Tem algo errado no filtro ou ordenação.";
}
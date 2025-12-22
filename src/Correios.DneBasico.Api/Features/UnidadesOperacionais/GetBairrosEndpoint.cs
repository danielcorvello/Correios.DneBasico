namespace Correios.DneBasico.Api.Features.UnidadesOperacionais;

/// <summary>
/// Retorna uma lista paginada de unidades operacionais, com suporte a filtragem e ordenação.
/// </summary>
public sealed class GetUnidadesOperacionaisEndpoint : Endpoint<GetUnidadesOperacionaisRequest, PagedResponse<GetUnidadesOperacionaisResponse>>
{
    private readonly DneBasicoDbContext _dbContext;

    public GetUnidadesOperacionaisEndpoint(DneBasicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get($"/{ApiConstants.RouteNames.UNIDADES_OPERACIONAIS}");
        AllowAnonymous();

        Description(b => b
            .WithTags(ApiConstants.Tags.BASE)
            .Accepts<GetUnidadesOperacionaisRequest>()
            .Produces<PagedResponse<GetUnidadesOperacionaisResponse>>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .WithDisplayName(nameof(GetUnidadesOperacionaisEndpoint)),
        clearDefaults: true);

        var mapper = new GetUnidadesOperacionaisRequestMapper();
        var filterableFields = mapper.GetMappingList();
        var orderableFields = mapper.GetMappingList();

        Summary(s =>
        {
            s.RequestParam(r => r.OrderBy,
                string.Format(ApiConstants.FILTER_SUMMARY, filterableFields));
            s.RequestParam(r => r.Filter,
                string.Format(ApiConstants.ORDERBY_SUMMARY, orderableFields));
        });
    }

    public override async Task HandleAsync(GetUnidadesOperacionaisRequest Request,
                                           CancellationToken ct)
    {
        var gridifyQuery = new GridifyQuery
        {
            Filter = Request.Filter,
            OrderBy = Request.OrderBy ?? nameof(GetUnidadesOperacionaisResponse.Nome),
            Page = Request.PageNumber ?? 1,
            PageSize = Request.PageSize ?? 100
        };
        var mapper = new GetUnidadesOperacionaisRequestMapper();

        var isValid = gridifyQuery.IsValid(mapper);
        if (!isValid)
        {
            AddError(ApiConstants.GRIDYFY_INVALID_QUERY);
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var query = _dbContext.UnidadesOperacionais
            .AsNoTracking()
            .ApplyFiltering(gridifyQuery.Filter, mapper)
            .ApplyOrdering(gridifyQuery.OrderBy, mapper)
            .Select(c => new GetUnidadesOperacionaisResponse
            {
                Id = c.Id,
                Uf = c.Uf,
                Nome = c.Nome,
                Cep = c.Cep,
                LocalidadeId = c.LocalidadeId,
                Endereco = c.Endereco,
                Localidade = c.Localidade.Nome,
                BairroId = c.BairroId,
                Bairro = c.Bairro.Nome,
                CaixasPostais = c.CaixasPostais,
                LogradouroId = c.LogradouroId,
                Logradouro = c.Logradouro != null ? c.Logradouro.Nome : null,
                NomeAbreviado = c.NomeAbreviado,
                FaixasCaixaPostal = c.FaixasCaixaPostal.Select(f => new FaixaCaixaPostalResponse(f.CaixaPostalInicial, f.CaixaPostalFinal)).ToList()
            });

        var totalCount = await query.CountAsync(ct);

        query = query.ApplyPaging(gridifyQuery);

        var result = await query.ToPagedResultAsync(
            totalCount,
            gridifyQuery.Page,
            gridifyQuery.PageSize,
            ct);

        await Send.OkAsync(result, cancellation: ct);
        return;
    }
}

public record GetUnidadesOperacionaisRequest : QueryListRequest
{
}

public class GetUnidadesOperacionaisRequestValidator : Validator<GetUnidadesOperacionaisRequest>
{
    public GetUnidadesOperacionaisRequestValidator()
    {
        Include(new QueryListRequestValidator());
    }
}

/// <summary>
/// Response para retornar os bairros.
/// </summary>
public record GetUnidadesOperacionaisResponse
{
    /// <summary>
    /// Chave da UOP
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome da UOP
    /// </summary>
    public string Nome { get; set; } = default!;

    /// <summary>
    /// CEP da UOP
    /// </summary>
    public string Cep { get; set; } = default!;

    /// <summary>
    /// Abreviatura do nome da unid. operacional (opcional)
    /// </summary>
    public string? NomeAbreviado { get; set; }

    /// <summary>
    /// Endereço da UOP
    /// </summary>
    public string Endereco { get; set; } = default!;

    /// <summary>
    /// Logradouro
    /// </summary>
    public string? Logradouro { get; set; }

    /// <summary>
    /// Localidade
    /// </summary>
    public string Localidade { get; set; } = default!;

    /// <summary>
    /// Sigla da UF
    /// </summary>
    public string Uf { get; set; } = default!;

    /// <summary>
    /// Bairro
    /// </summary>
    public string Bairro { get; set; } = default!;

    /// <summary>
    /// Indicador de caixa postal (S ou N)
    /// </summary>
    public string CaixasPostais { get; set; } = default!;

    /// <summary>
    /// Faixas de Caixa Postal - UOP
    /// </summary>
    public ICollection<FaixaCaixaPostalResponse> FaixasCaixaPostal { get; set; } = [];

    /// <summary>
    /// Chave da localidade
    /// </summary>
    public int LocalidadeId { get; set; }

    /// <summary>
    /// Chave do bairro
    /// </summary>
    public int BairroId { get; set; }

    /// <summary>
    /// Chave do logradouro (opcional)
    /// </summary>
    public int? LogradouroId { get; set; }
};

class GetUnidadesOperacionaisRequestMapper : GridifyMapper<UnidadeOperacional>
{
    public GetUnidadesOperacionaisRequestMapper()
    {
        ClearMappings();

        AddMap(nameof(GetUnidadesOperacionaisResponse.Id), o => o.Id);
        AddMap(nameof(GetUnidadesOperacionaisResponse.Nome), o => o.Nome);
        AddMap(nameof(GetUnidadesOperacionaisResponse.Cep), o => o.Cep);
        AddMap(nameof(GetUnidadesOperacionaisResponse.NomeAbreviado), o => o.NomeAbreviado);
        AddMap(nameof(GetUnidadesOperacionaisResponse.Endereco), o => o.Endereco);
        AddMap(nameof(GetUnidadesOperacionaisResponse.Localidade), o => o.Localidade.Nome);
        AddMap(nameof(GetUnidadesOperacionaisResponse.Uf), o => o.Uf);
        AddMap(nameof(GetUnidadesOperacionaisResponse.Bairro), o => o.Bairro.Nome);
        AddMap(nameof(GetUnidadesOperacionaisResponse.CaixasPostais), o => o.CaixasPostais);

        AddMap(nameof(GetUnidadesOperacionaisResponse.LocalidadeId), o => o.LocalidadeId);
        AddMap(nameof(GetUnidadesOperacionaisResponse.BairroId), o => o.BairroId);
        AddMap(nameof(GetUnidadesOperacionaisResponse.LogradouroId), o => o.LogradouroId);
    }
}
namespace Correios.DneBasico.Api.Features.CaixasPostaisComunitarias;

/// <summary>
/// Retorna uma lista paginada de caixas postais comunitárias, com suporte a filtragem e ordenação.
/// </summary>
public sealed class GetCaixasPostaisComunitariasEndpoint : Endpoint<GetCaixasPostaisComunitariasRequest, PagedResponse<GetCaixasPostaisComunitariasResponse>>
{
    private readonly DneBasicoDbContext _dbContext;

    public GetCaixasPostaisComunitariasEndpoint(DneBasicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get($"/{ApiConstants.RouteNames.CAIXAS_POSTAIS_COMUNITARIAS}");
        AllowAnonymous();

        Description(b => b
            .WithTags(ApiConstants.Tags.BASE)
            .Accepts<GetCaixasPostaisComunitariasRequest>()
            .Produces<PagedResponse<GetCaixasPostaisComunitariasResponse>>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .WithDisplayName(nameof(GetCaixasPostaisComunitariasEndpoint)),
        clearDefaults: true);

        var mapper = new GetCaixasPostaisComunitariasRequestMapper();
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

    public override async Task HandleAsync(GetCaixasPostaisComunitariasRequest Request,
                                           CancellationToken ct)
    {
        var gridifyQuery = new GridifyQuery
        {
            Filter = Request.Filter,
            OrderBy = Request.OrderBy ?? nameof(GetCaixasPostaisComunitariasResponse.Nome),
            Page = Request.PageNumber ?? 1,
            PageSize = Request.PageSize ?? 100
        };
        var mapper = new GetCaixasPostaisComunitariasRequestMapper();

        var isValid = gridifyQuery.IsValid(mapper);
        if (!isValid)
        {
            AddError(ApiConstants.GRIDYFY_INVALID_QUERY);
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var query = _dbContext.CaixasPostaisComunitarias
            .AsNoTracking()
            .ApplyFiltering(gridifyQuery.Filter, mapper)
            .ApplyOrdering(gridifyQuery.OrderBy, mapper)
            .Select(c => new GetCaixasPostaisComunitariasResponse
            {
                Id = c.Id,
                Uf = c.Uf,
                Nome = c.Nome,
                Cep = c.Cep,
                Endereco = c.Endereco,
                Faixas = c.Faixas!
                        .Select(f => new FaixaCaixaPostalResponse(
                            f.CaixaPostalInicial,
                            f.CaixaPostalFinal))
                        .ToList(),
                LocalidadeId = c.LocalidadeId,
                Localidade = c.Localidade.Nome
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

public record GetCaixasPostaisComunitariasRequest : QueryListRequest
{
}

public class GetCaixasPostaisComunitariasRequestValidator : Validator<GetCaixasPostaisComunitariasRequest>
{
    public GetCaixasPostaisComunitariasRequestValidator()
    {
        Include(new QueryListRequestValidator());
    }
}

/// <summary>
/// Response para retornar as caixas postais comunitárias.
/// </summary>
public record GetCaixasPostaisComunitariasResponse
{
    /// <summary>
    /// Chave da caixa postal comunitária
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome da CPC
    /// </summary>
    public string Nome { get; set; } = default!;

    /// <summary>
    /// Endereço da CPC
    /// </summary>
    public string Endereco { get; set; } = default!;

    /// <summary>
    /// CEP da CPC
    /// </summary>
    public string Cep { get; set; } = default!;

    /// <summary>
    /// Localidade
    /// </summary>
    public string Localidade { get; set; } = default!;

    /// <summary>
    /// Sigla da UF
    /// </summary>
    public string Uf { get; set; } = default!;

    /// <summary>
    /// Faixas de Caixa Postal Comunitária
    /// </summary>
    public ICollection<FaixaCaixaPostalResponse> Faixas { get; set; } = [];

    /// <summary>
    /// Chave da localidade
    /// </summary>
    public int LocalidadeId { get; set; }
};

class GetCaixasPostaisComunitariasRequestMapper : GridifyMapper<CaixaPostalComunitaria>
{
    public GetCaixasPostaisComunitariasRequestMapper()
    {
        ClearMappings();

        AddMap(nameof(GetCaixasPostaisComunitariasResponse.Id), o => o.Id);
        AddMap(nameof(GetCaixasPostaisComunitariasResponse.Nome), o => o.Nome);
        AddMap(nameof(GetCaixasPostaisComunitariasResponse.Endereco), o => o.Endereco);
        AddMap(nameof(GetCaixasPostaisComunitariasResponse.Cep), o => o.Cep);
        AddMap(nameof(GetCaixasPostaisComunitariasResponse.Localidade), o => o.Localidade.Nome);
        AddMap(nameof(GetCaixasPostaisComunitariasResponse.Uf), o => o.Uf);
        AddMap(nameof(GetCaixasPostaisComunitariasResponse.LocalidadeId), o => o.LocalidadeId);
    }
}
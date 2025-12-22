namespace Correios.DneBasico.Api.Features.Paises;

/// <summary>
/// Retorna uma lista paginada de países, com suporte a filtragem e ordenação.
/// </summary>
public sealed class GetPaisesEndpoint : Endpoint<GetPaisesRequest, PagedResponse<GetPaisesResponse>>
{
    private readonly DneBasicoDbContext _dbContext;

    public GetPaisesEndpoint(DneBasicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get($"/{ApiConstants.RouteNames.PAISES}");
        AllowAnonymous();

        Description(b => b
            .WithTags(ApiConstants.Tags.BASE)
            .Accepts<GetPaisesRequest>()
            .Produces<PagedResponse<GetPaisesResponse>>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .WithDisplayName(nameof(GetPaisesEndpoint)),
        clearDefaults: true);

        var mapper = new GetPaisesRequestMapper();
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

    public override async Task HandleAsync(GetPaisesRequest Request,
                                           CancellationToken ct)
    {
        var gridifyQuery = new GridifyQuery
        {
            Filter = Request.Filter,
            OrderBy = Request.OrderBy ?? nameof(GetPaisesResponse.Sigla),
            Page = Request.PageNumber ?? 1,
            PageSize = Request.PageSize ?? 100
        };
        var mapper = new GetPaisesRequestMapper();

        var isValid = gridifyQuery.IsValid(mapper);
        if (!isValid)
        {
            AddError(ApiConstants.GRIDYFY_INVALID_QUERY);
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var query = _dbContext.Paises
            .AsNoTracking()
            .ApplyFiltering(gridifyQuery.Filter, mapper)
            .ApplyOrdering(gridifyQuery.OrderBy, mapper)
            .Select(c => new GetPaisesResponse
            {
                Sigla = c.Sigla,
                SiglaAlternativa = c.SiglaAlternativa,
                NomePortugues = c.NomePortugues,
                NomeIngles = c.NomeIngles,
                NomeFrances = c.NomeFrances
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

public record GetPaisesRequest : QueryListRequest
{
}

public class GetPaisesRequestValidator : Validator<GetPaisesRequest>
{
    public GetPaisesRequestValidator()
    {
        Include(new QueryListRequestValidator());
    }
}

public record GetPaisesResponse
{
    /// <summary>
    /// Sigla do País
    /// </summary>
    public string Sigla { get; set; } = default!;

    /// <summary>
    /// Sigla Alternativa do País
    /// </summary>
    public string SiglaAlternativa { get; set; } = default!;

    /// <summary>
    /// Nome do País em Português
    /// </summary>
    public string NomePortugues { get; set; } = default!;

    /// <summary>
    /// Nome do País em Inglês
    /// </summary>
    public string NomeIngles { get; set; } = default!;

    /// <summary>
    /// Nome do País em Francês
    /// </summary>
    public string NomeFrances { get; set; } = default!;
}

class GetPaisesRequestMapper : GridifyMapper<Pais>
{
    public GetPaisesRequestMapper()
    {
        ClearMappings();

        AddMap(nameof(GetPaisesResponse.Sigla), o => o.Sigla);
        AddMap(nameof(GetPaisesResponse.SiglaAlternativa), o => o.SiglaAlternativa);
        AddMap(nameof(GetPaisesResponse.NomePortugues), o => o.NomePortugues);
        AddMap(nameof(GetPaisesResponse.NomeIngles), o => o.NomeIngles);
        AddMap(nameof(GetPaisesResponse.NomeFrances), o => o.NomeFrances);
    }
}
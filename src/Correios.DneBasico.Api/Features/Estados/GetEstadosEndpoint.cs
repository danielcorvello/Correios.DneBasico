namespace Correios.DneBasico.Api.Features.Estados;

/// <summary>
/// Retorna uma lista paginada de estados, com suporte a filtragem e ordenação.
/// </summary>
public sealed class GetEstadosEndpoint : Endpoint<GetEstadosRequest, PagedResponse<GetEstadosResponse>>
{
    private readonly DneBasicoDbContext _dbContext;

    public GetEstadosEndpoint(DneBasicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get($"/{ApiConstants.RouteNames.ESTADOS}");
        AllowAnonymous();

        Description(b => b
            .WithTags(ApiConstants.Tags.BASE)
            .Accepts<GetEstadosRequest>()
            .Produces<PagedResponse<GetEstadosResponse>>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .WithDisplayName(nameof(GetEstadosEndpoint)),
        clearDefaults: true);

        var mapper = new GetEstadosRequestMapper();
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

    public override async Task HandleAsync(GetEstadosRequest Request,
                                           CancellationToken ct)
    {
        var gridifyQuery = new GridifyQuery
        {
            Filter = Request.Filter,
            OrderBy = Request.OrderBy ?? nameof(GetEstadosResponse.Nome),
            Page = Request.PageNumber ?? 1,
            PageSize = Request.PageSize ?? 100
        };
        var mapper = new GetEstadosRequestMapper();

        var isValid = gridifyQuery.IsValid(mapper);
        if (!isValid)
        {
            AddError(ApiConstants.GRIDYFY_INVALID_QUERY);
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var query = _dbContext.Estados
            .AsNoTracking()
            .ApplyFiltering(gridifyQuery.Filter, mapper)
            .ApplyOrdering(gridifyQuery.OrderBy, mapper)
            .Select(c => new GetEstadosResponse(c.Ibge, c.Nome, c.Uf));

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

public record GetEstadosRequest : QueryListRequest
{
}

public class GetEstadosRequestValidator : Validator<GetEstadosRequest>
{
    public GetEstadosRequestValidator()
    {
        Include(new QueryListRequestValidator());
    }
}

/// <summary>
/// Response para retornar os estados.
/// </summary>
public record GetEstadosResponse(string Ibge, string Nome, string Uf);

class GetEstadosRequestMapper : GridifyMapper<Estado>
{
    public GetEstadosRequestMapper()
    {
        ClearMappings();

        AddMap(nameof(GetEstadosResponse.Ibge), o => o.Ibge);
        AddMap(nameof(GetEstadosResponse.Nome), o => o.Nome);
        AddMap(nameof(GetEstadosResponse.Uf), o => o.Uf);
    }
}
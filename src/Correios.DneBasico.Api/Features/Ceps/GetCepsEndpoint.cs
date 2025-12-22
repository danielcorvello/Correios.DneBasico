using Correios.DneBasico.Domain.Enums;

namespace Correios.DneBasico.Api.Features.Ceps;

/// <summary>
/// Retorna uma lista paginada de CEPs, com suporte a filtragem e ordenação.
/// </summary>
public sealed class GetCepsEndpoint : Endpoint<GetCepsRequest, PagedResponse<GetCepsResponse>>
{
    private readonly DneBasicoDbContext _dbContext;

    public GetCepsEndpoint(DneBasicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get($"/{ApiConstants.RouteNames.CEPS}");
        AllowAnonymous();

        Description(b => b
            .WithTags(ApiConstants.Tags.BASE)
            .Accepts<GetCepsRequest>()
            .Produces<PagedResponse<GetCepsResponse>>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .WithDisplayName(nameof(GetCepsEndpoint)),
        clearDefaults: true);

        var mapper = new GetCepsRequestMapper();
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

    public override async Task HandleAsync(GetCepsRequest Request,
                                           CancellationToken ct)
    {
        var gridifyQuery = new GridifyQuery
        {
            Filter = Request.Filter,
            OrderBy = Request.OrderBy ?? nameof(GetCepsResponse.Codigo),
            Page = Request.PageNumber ?? 1,
            PageSize = Request.PageSize ?? 100
        };
        var mapper = new GetCepsRequestMapper();

        var isValid = gridifyQuery.IsValid(mapper);
        if (!isValid)
        {
            AddError(ApiConstants.GRIDYFY_INVALID_QUERY);
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var query = _dbContext.Ceps
            .AsNoTracking()
            .ApplyFiltering(gridifyQuery.Filter, mapper)
            .ApplyOrdering(gridifyQuery.OrderBy, mapper)
            .Select(cep => new GetCepsResponse(
                cep.Codigo,
                cep.Ibge,
                cep.Municipio,
                cep.Uf,
                cep.Bairro,
                cep.Distrito,
                cep.TipoLogradouro,
                cep.Logradouro,
                cep.LogradouroCompleto,
                cep.Complemento,
                cep.Unidade,
                cep.Geral,
                cep.Tipo));

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

/// <summary>
/// Request para obter a lista de ceps.
/// </summary>
public record GetCepsRequest : QueryListRequest
{
}

public class GetCepsRequestValidator : Validator<GetCepsRequest>
{
    public GetCepsRequestValidator()
    {
        Include(new QueryListRequestValidator());
    }
}

/// <summary>
/// Response para retornar os ceps.
/// </summary>
public record GetCepsResponse(
    string Codigo,
    string Ibge,
    string Municipio,
    string Uf,
    string? Bairro,
    string? Distrito,
    string? TipoLogradouro,
    string? Logradouro,
    string? LogradouroCompleto,
    string? Complemento,
    string? Unidade,
    bool Geral,
    TipoCep Tipo);

class GetCepsRequestMapper : GridifyMapper<Cep>
{
    public GetCepsRequestMapper()
    {
        ClearMappings();

        AddMap(nameof(GetCepsResponse.Codigo), o => o.Codigo);
        AddMap(nameof(GetCepsResponse.Ibge), o => o.Ibge);
        AddMap(nameof(GetCepsResponse.Municipio), o => o.Municipio);
        AddMap(nameof(GetCepsResponse.Uf), o => o.Uf);
        AddMap(nameof(GetCepsResponse.Bairro), o => o.Bairro);
        AddMap(nameof(GetCepsResponse.Distrito), o => o.Distrito);
        AddMap(nameof(GetCepsResponse.TipoLogradouro), o => o.TipoLogradouro);
        AddMap(nameof(GetCepsResponse.Logradouro), o => o.Logradouro);
        AddMap(nameof(GetCepsResponse.LogradouroCompleto), o => o.LogradouroCompleto);
        AddMap(nameof(GetCepsResponse.Complemento), o => o.Complemento);
        AddMap(nameof(GetCepsResponse.Unidade), o => o.Unidade);
        AddMap(nameof(GetCepsResponse.Geral), o => o.Geral);
        AddMap(nameof(GetCepsResponse.Tipo), o => o.Tipo);
    }
}
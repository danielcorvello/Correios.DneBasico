using Correios.DneBasico.Domain.Enums;

namespace Correios.DneBasico.Api.Features.Cidades;

/// <summary>
/// Retorna uma lista paginada de cidades, com suporte a filtragem e ordenação.
/// </summary>
public sealed class GetCidadesEndpoint : Endpoint<GetCidadesRequest, PagedResponse<GetCidadesResponse>>
{
    private readonly DneBasicoDbContext _dbContext;

    public GetCidadesEndpoint(DneBasicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get($"/{ApiConstants.RouteNames.ESTADOS}/{{uf}}/{ApiConstants.RouteNames.CIDADES}");
        AllowAnonymous();

        Description(b => b
            .WithTags(ApiConstants.Tags.BASE)
            .Accepts<GetCidadesRequest>()
            .Produces<PagedResponse<GetCidadesResponse>>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .WithDisplayName(nameof(GetCidadesEndpoint)),
        clearDefaults: true);

        var mapper = new GetCidadesRequestMapper();
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

    public override async Task HandleAsync(GetCidadesRequest Request,
                                           CancellationToken ct)
    {
        var gridifyQuery = new GridifyQuery
        {
            Filter = Request.Filter,
            OrderBy = Request.OrderBy ?? nameof(GetCidadesResponse.Nome),
            Page = Request.PageNumber ?? 1,
            PageSize = Request.PageSize ?? 100
        };
        var mapper = new GetCidadesRequestMapper();

        var isValid = gridifyQuery.IsValid(mapper);
        if (!isValid)
        {
            AddError(ApiConstants.GRIDYFY_INVALID_QUERY);
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var query = _dbContext.Localidades
            .AsNoTracking()
            .Where(c => c.Uf.ToUpper() == Request.Uf.ToUpper() &&
                        c.Ibge != null &&
                        c.Tipo == TipoLocalidade.MUNICIPIO)
            .ApplyFiltering(gridifyQuery.Filter, mapper)
            .ApplyOrdering(gridifyQuery.OrderBy, mapper)
            .Select(c => new GetCidadesResponse
            {
                Id = c.Id,
                Uf = c.Uf,
                Nome = c.Nome,
                NomeAbreviado = c.NomeAbreviado,
                Ibge = c.Ibge
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

public record GetCidadesRequest : QueryListRequest
{
    /// <summary>
    /// Sigla da UF para filtrar as cidades.
    /// </summary>
    public string Uf { get; set; } = default!;
}

public class GetCidadesRequestValidator : Validator<GetCidadesRequest>
{
    public GetCidadesRequestValidator()
    {
        Include(new QueryListRequestValidator());

        RuleFor(r => r.Uf)
            .NotEmpty()
            .WithMessage("A sigla da UF é obrigatória.")
            .Length(2)
            .WithMessage("A sigla da UF deve conter 2 caracteres.");
    }
}

/// <summary>
/// Response para retornar as cidades
/// </summary>
public record GetCidadesResponse
{
    /// <summary>
    /// chave da localidade
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// sigla da UF
    /// </summary>
    public string Uf { get; set; } = default!;

    /// <summary>
    /// Nome da localidade
    /// </summary>
    public string Nome { get; set; } = default!;

    /// <summary>
    /// Abreviatura do nome da localidade
    /// </summary>
    public string? NomeAbreviado { get; set; }

    /// <summary>
    /// Código IBGE do município
    /// </summary>
    public string? Ibge { get; set; }
};

class GetCidadesRequestMapper : GridifyMapper<Localidade>
{
    public GetCidadesRequestMapper()
    {
        ClearMappings();

        AddMap(nameof(GetCidadesResponse.Id), o => o.Id);
        AddMap(nameof(GetCidadesResponse.Uf), o => o.Uf);
        AddMap(nameof(GetCidadesResponse.Nome), o => o.Nome);
        AddMap(nameof(GetCidadesResponse.NomeAbreviado), o => o.NomeAbreviado);
        AddMap(nameof(GetCidadesResponse.Ibge), o => o.Ibge);
    }
}
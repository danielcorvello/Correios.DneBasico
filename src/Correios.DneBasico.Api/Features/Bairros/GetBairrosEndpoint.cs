namespace Correios.DneBasico.Api.Features.Bairros;

/// <summary>
/// Retorna uma lista paginada de bairros, com suporte a filtragem e ordenação.
/// </summary>
public sealed class GetBairrosEndpoint : Endpoint<GetBairrosRequest, PagedResponse<GetBairrosResponse>>
{
    private readonly DneBasicoDbContext _dbContext;

    public GetBairrosEndpoint(DneBasicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get($"/{ApiConstants.RouteNames.BAIRROS}");
        AllowAnonymous();

        Description(b => b
            .WithTags(ApiConstants.Tags.BASE)
            .Accepts<GetBairrosRequest>()
            .Produces<PagedResponse<GetBairrosResponse>>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .WithDisplayName(nameof(GetBairrosEndpoint)),
        clearDefaults: true);

        var mapper = new GetBairrosRequestMapper();
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

    public override async Task HandleAsync(GetBairrosRequest Request,
                                           CancellationToken ct)
    {
        var gridifyQuery = new GridifyQuery
        {
            Filter = Request.Filter,
            OrderBy = Request.OrderBy ?? nameof(GetBairrosResponse.Nome),
            Page = Request.PageNumber ?? 1,
            PageSize = Request.PageSize ?? 100
        };
        var mapper = new GetBairrosRequestMapper();

        var isValid = gridifyQuery.IsValid(mapper);
        if (!isValid)
        {
            AddError(ApiConstants.GRIDYFY_INVALID_QUERY);
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var query = _dbContext.Bairros
            .AsNoTracking()
            .ApplyFiltering(gridifyQuery.Filter, mapper)
            .ApplyOrdering(gridifyQuery.OrderBy, mapper)
            .Select(c => new GetBairrosResponse
            {
                Id = c.Id,
                Uf = c.Uf,
                Nome = c.Nome,
                NomeAbreviado = c.NomeAbreviado,
                LocalidadeId = c.LocalidadeId,
                Localidade = c.Localidade.Nome,
                Variacoes = c.Variacoes!
                                .Select(v => v.Denominacao)
                                .ToList(),
                FaixasCep = c.Faixas!
                                .Select(f => new FaixaCepResponse(
                                    f.CepInicial,
                                    f.CepFinal))
                                .ToList()
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

public record GetBairrosRequest : QueryListRequest
{
}

public class GetBairrosRequestValidator : Validator<GetBairrosRequest>
{
    public GetBairrosRequestValidator()
    {
        Include(new QueryListRequestValidator());
    }
}

/// <summary>
/// Response para retornar as bairros.
/// </summary>
public record GetBairrosResponse
{
    /// <summary>
    /// Chave do bairro
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome do bairro
    /// </summary>
    public string Nome { get; set; } = default!;

    /// <summary>
    /// Abreviatura do nome do bairro
    /// </summary>
    public string? NomeAbreviado { get; set; }

    /// <summary>
    /// Nome da localidade
    /// </summary>
    public string Localidade { get; set; } = default!;

    /// <summary>
    /// Sigla da UF
    /// </summary>
    public string Uf { get; set; } = default!;

    /// <summary>
    /// Variações de nome do Bairro
    /// </summary>
    public ICollection<string> Variacoes { get; set; } = [];

    /// <summary>
    /// Faixas de CEP do Bairro
    /// </summary>
    public ICollection<FaixaCepResponse> FaixasCep { get; set; } = [];

    /// <summary>
    /// Chave da localidade
    /// </summary>
    public int LocalidadeId { get; set; }
};

class GetBairrosRequestMapper : GridifyMapper<Bairro>
{
    public GetBairrosRequestMapper()
    {
        ClearMappings();

        AddMap(nameof(GetBairrosResponse.Id), o => o.Id);
        AddMap(nameof(GetBairrosResponse.Nome), o => o.Nome);
        AddMap(nameof(GetBairrosResponse.Uf), o => o.Uf);
        AddMap(nameof(GetBairrosResponse.LocalidadeId), o => o.LocalidadeId);
    }
}
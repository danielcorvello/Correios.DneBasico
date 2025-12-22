using Correios.DneBasico.Domain.Enums;

namespace Correios.DneBasico.Api.Features.Localidades;

/// <summary>
/// Retorna uma lista paginada de localidades, com suporte a filtragem e ordenação.
/// </summary>
public sealed class GetLocalidadesEndpoint : Endpoint<GetLocalidadesRequest, PagedResponse<GetLocalidadesResponse>>
{
    private readonly DneBasicoDbContext _dbContext;

    public GetLocalidadesEndpoint(DneBasicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get($"/{ApiConstants.RouteNames.LOCALIDADES}");
        AllowAnonymous();

        Description(b => b
            .WithTags(ApiConstants.Tags.BASE)
            .Accepts<GetLocalidadesRequest>()
            .Produces<PagedResponse<GetLocalidadesResponse>>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .WithDisplayName(nameof(GetLocalidadesEndpoint)),
        clearDefaults: true);

        var mapper = new GetLocalidadesRequestMapper();
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

    public override async Task HandleAsync(GetLocalidadesRequest Request,
                                           CancellationToken ct)
    {
        var gridifyQuery = new GridifyQuery
        {
            Filter = Request.Filter,
            OrderBy = Request.OrderBy ?? nameof(GetLocalidadesResponse.Nome),
            Page = Request.PageNumber ?? 1,
            PageSize = Request.PageSize ?? 100
        };
        var mapper = new GetLocalidadesRequestMapper();

        var isValid = gridifyQuery.IsValid(mapper);
        if (!isValid)
        {
            AddError(ApiConstants.GRIDYFY_INVALID_QUERY);
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var query = _dbContext.Localidades
            .AsNoTracking()
            .ApplyFiltering(gridifyQuery.Filter, mapper)
            .ApplyOrdering(gridifyQuery.OrderBy, mapper)
            .Select(c => new GetLocalidadesResponse
            {
                Id = c.Id,
                Uf = c.Uf,
                Nome = c.Nome,
                Cep = c.Cep,
                Situacao = c.Situacao,
                Tipo = c.Tipo,
                SubordinacaoId = c.SubordinacaoId,
                Subordinacao = c.Subordinacao != null ? c.Subordinacao.Nome : null,
                NomeAbreviado = c.NomeAbreviado,
                Ibge = c.Ibge,
                Variacoes = c.Variacoes!.Select(v => v.Denominacao).ToList()
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

public record GetLocalidadesRequest : QueryListRequest
{
}

public class GetLocalidadesRequestValidator : Validator<GetLocalidadesRequest>
{
    public GetLocalidadesRequestValidator()
    {
        Include(new QueryListRequestValidator());
    }
}

/// <summary>
/// Response para retornar os bairros.
/// </summary>
public record GetLocalidadesResponse
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
    /// CEP da localidade (para localidade não codificada, ou seja Situacao = 0 | NAO_CODIFICADA)
    /// </summary>
    public string? Cep { get; set; }

    /// <summary>
    /// Situação da localidade:
    /// </summary>
    public SituacaoLocalidade Situacao { get; set; } = default!;

    /// <summary>
    /// Tipo de localidade
    /// </summary>
    public TipoLocalidade Tipo { get; set; } = default!;

    /// <summary>
    /// Nome da localidade de subordinação
    /// </summary>
    public string? Subordinacao { get; set; }

    /// <summary>
    /// Código do município IBGE
    /// </summary>
    public string? Ibge { get; set; }

    /// <summary>
    /// Variações de nome da Localidade
    /// </summary>
    public ICollection<string> Variacoes { get; set; } = [];

    /// <summary>
    /// Chave da localidade de subordinação
    /// </summary>
    public int? SubordinacaoId { get; set; }
};

class GetLocalidadesRequestMapper : GridifyMapper<Localidade>
{
    public GetLocalidadesRequestMapper()
    {
        ClearMappings();

        AddMap(nameof(GetLocalidadesResponse.Id), o => o.Id);
        AddMap(nameof(GetLocalidadesResponse.Uf), o => o.Uf);
        AddMap(nameof(GetLocalidadesResponse.Nome), o => o.Nome);
        AddMap(nameof(GetLocalidadesResponse.Cep), o => o.Cep);
        AddMap(nameof(GetLocalidadesResponse.Situacao), o => o.Situacao);
        AddMap(nameof(GetLocalidadesResponse.Tipo), o => o.Tipo);
        AddMap(nameof(GetLocalidadesResponse.SubordinacaoId), o => o.SubordinacaoId);
        AddMap(nameof(GetLocalidadesResponse.Subordinacao), o => o.Subordinacao!.Nome);
        AddMap(nameof(GetLocalidadesResponse.NomeAbreviado), o => o.NomeAbreviado);
        AddMap(nameof(GetLocalidadesResponse.Ibge), o => o.Ibge);
    }
}
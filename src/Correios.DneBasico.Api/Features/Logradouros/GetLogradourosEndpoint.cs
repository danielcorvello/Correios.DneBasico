using Correios.DneBasico.Domain.Enums;

namespace Correios.DneBasico.Api.Features.Logradouros;

/// <summary>
/// Retorna uma lista paginada de logradouros, com suporte a filtragem e ordenação.
/// </summary>
public sealed class GetLogradourosEndpoint : Endpoint<GetLogradourosRequest, PagedResponse<GetLogradourosResponse>>
{
    private readonly DneBasicoDbContext _dbContext;

    public GetLogradourosEndpoint(DneBasicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get($"/{ApiConstants.RouteNames.LOGRADOUROS}");
        AllowAnonymous();

        Description(b => b
            .WithTags(ApiConstants.Tags.BASE)
            .Accepts<GetLogradourosRequest>()
            .Produces<PagedResponse<GetLogradourosResponse>>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .WithDisplayName(nameof(GetLogradourosEndpoint)),
        clearDefaults: true);

        var mapper = new GetLogradourosRequestMapper();
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

    public override async Task HandleAsync(GetLogradourosRequest Request,
                                           CancellationToken ct)
    {
        var gridifyQuery = new GridifyQuery
        {
            Filter = Request.Filter,
            OrderBy = Request.OrderBy ?? nameof(GetLogradourosResponse.Nome),
            Page = Request.PageNumber ?? 1,
            PageSize = Request.PageSize ?? 100
        };
        var mapper = new GetLogradourosRequestMapper();

        var isValid = gridifyQuery.IsValid(mapper);
        if (!isValid)
        {
            AddError(ApiConstants.GRIDYFY_INVALID_QUERY);
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var query = _dbContext.Logradouros
            .AsNoTracking()
            .ApplyFiltering(gridifyQuery.Filter, mapper)
            .ApplyOrdering(gridifyQuery.OrderBy, mapper)
            .Select(c => new GetLogradourosResponse
            {
                Id = c.Id,
                Uf = c.Uf,
                Nome = c.Nome,
                Cep = c.Cep,
                Bairro = c.Bairro.Nome,
                BairroId = c.BairroId,
                Complemento = c.Complemento,
                FaixasNumericasSeccionamento = c.FaixasNumericasSeccionamento!.Select(f => new FaixaNumericaSeccionamentoResponse(f.SeccionamentoInicial, f.SeccionamentoFinal, f.ParidadeLado)).ToList(),
                Localidade = c.Localidade.Nome,
                LocalidadeId = c.LocalidadeId,
                NomeAbreviado = c.NomeAbreviado,
                StatusTipo = c.StatusTipo,
                Tipo = c.Tipo,
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

public record GetLogradourosRequest : QueryListRequest
{
}

public class GetLogradourosRequestValidator : Validator<GetLogradourosRequest>
{
    public GetLogradourosRequestValidator()
    {
        Include(new QueryListRequestValidator());
    }
}

/// <summary>
/// Response para retornar os bairros.
/// </summary>
public record GetLogradourosResponse
{
    /// <summary>
    /// Chave do logradouro
    /// </summary>
    public int Id { get; set; }


    /// <summary>
    /// CEP do logradouro
    /// </summary>
    public string Cep { get; set; } = default!;

    /// <summary>
    /// Tipo de logradouro
    /// </summary>
    public string Tipo { get; set; } = default!;

    /// <summary>
    /// Nome do logradouro
    /// </summary>
    public string Nome { get; set; } = default!;

    /// <summary>
    /// Abreviatura do nome do logradouro (opcional)
    /// </summary>
    public string? NomeAbreviado { get; set; }

    /// <summary>
    /// Complemento do logradouro (opcional)
    /// </summary>
    public string? Complemento { get; set; }

    /// <summary>
    /// Localidade
    /// </summary>
    public string Localidade { get; set; } = default!;

    /// <summary>
    /// Bairro do logradouro
    /// </summary>
    public string Bairro { get; set; } = default!;

    /// <summary>
    /// Sigla da UF
    /// </summary>
    public string Uf { get; set; } = default!;

    /// <summary>
    /// Indicador de utilização do tipo de logradouro (S ou N) (opcional)
    /// </summary>
    public string? StatusTipo { get; set; }

    /// <summary>
    /// Variações do Logradouro
    /// </summary>
    public ICollection<string> Variacoes { get; set; } = [];

    /// <summary>
    /// Faixas Numéricas de Seccionamento do Logradouro
    /// </summary>
    public ICollection<FaixaNumericaSeccionamentoResponse> FaixasNumericasSeccionamento { get; set; } = [];

    /// <summary>
    /// Chave da localidade
    /// </summary>
    public int LocalidadeId { get; set; }

    /// <summary>
    /// Chave do bairro  do logradouro
    /// </summary>
    public int BairroId { get; set; }
};

public record FaixaNumericaSeccionamentoResponse(string Inicial, string Final, ParidadeLadoSeccionamento ParidadeLado);

class GetLogradourosRequestMapper : GridifyMapper<Logradouro>
{
    public GetLogradourosRequestMapper()
    {
        ClearMappings();

        AddMap(nameof(GetLogradourosResponse.Id), o => o.Id);
        AddMap(nameof(GetLogradourosResponse.Cep), o => o.Cep);
        AddMap(nameof(GetLogradourosResponse.Nome), o => o.Nome);
        AddMap(nameof(GetLogradourosResponse.NomeAbreviado), o => o.NomeAbreviado);
        AddMap(nameof(GetLogradourosResponse.Complemento), o => o.Complemento);
        AddMap(nameof(GetLogradourosResponse.Localidade), o => o.Localidade.Nome);
        AddMap(nameof(GetLogradourosResponse.Bairro), o => o.Bairro.Nome);
        AddMap(nameof(GetLogradourosResponse.Uf), o => o.Uf);
        AddMap(nameof(GetLogradourosResponse.StatusTipo), o => o.StatusTipo);
        AddMap(nameof(GetLogradourosResponse.Tipo), o => o.Tipo);

        AddMap(nameof(GetLogradourosResponse.LocalidadeId), o => o.LocalidadeId);
        AddMap(nameof(GetLogradourosResponse.BairroId), o => o.BairroId);
    }
}
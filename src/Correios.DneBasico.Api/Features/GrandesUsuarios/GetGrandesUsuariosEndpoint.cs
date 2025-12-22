namespace Correios.DneBasico.Api.Features.GrandesUsuarios;

/// <summary>
/// Retorna uma lista paginada de Grandes usuários, com suporte a filtragem e ordenação.
/// </summary>
public sealed class GetGrandesUsuariosEndpoint : Endpoint<GetGrandesUsuariosRequest, PagedResponse<GetGrandesUsuariosResponse>>
{
    private readonly DneBasicoDbContext _dbContext;

    public GetGrandesUsuariosEndpoint(DneBasicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get($"/{ApiConstants.RouteNames.GRANDES_USUARIOS}");
        AllowAnonymous();

        Description(b => b
            .WithTags(ApiConstants.Tags.BASE)
            .Accepts<GetGrandesUsuariosRequest>()
            .Produces<PagedResponse<GetGrandesUsuariosResponse>>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .WithDisplayName(nameof(GetGrandesUsuariosEndpoint)),
        clearDefaults: true);

        var mapper = new GetGrandesUsuariosRequestMapper();
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

    public override async Task HandleAsync(GetGrandesUsuariosRequest Request,
                                           CancellationToken ct)
    {
        var gridifyQuery = new GridifyQuery
        {
            Filter = Request.Filter,
            OrderBy = Request.OrderBy ?? nameof(GetGrandesUsuariosResponse.Nome),
            Page = Request.PageNumber ?? 1,
            PageSize = Request.PageSize ?? 100
        };
        var mapper = new GetGrandesUsuariosRequestMapper();

        var isValid = gridifyQuery.IsValid(mapper);
        if (!isValid)
        {
            AddError(ApiConstants.GRIDYFY_INVALID_QUERY);
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var query = _dbContext.GrandesUsuarios
            .AsNoTracking()
            .ApplyFiltering(gridifyQuery.Filter, mapper)
            .ApplyOrdering(gridifyQuery.OrderBy, mapper)
            .Select(c => new GetGrandesUsuariosResponse
            {
                Id = c.Id,
                Uf = c.Uf,
                Nome = c.Nome,
                NomeAbreviado = c.NomeAbreviado,
                LocalidadeId = c.LocalidadeId,
                Localidade = c.Localidade.Nome,
                BairroId = c.BairroId,
                Bairro = c.Bairro.Nome,
                Cep = c.Cep,
                Endereco = c.Endereco,
                LogradouroId = c.LogradouroId,
                Logradouro = c.Logradouro != null ? c.Logradouro.Nome : null,
                Complemento = c.Logradouro != null ? c.Logradouro.Complemento : null,
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

public record GetGrandesUsuariosRequest : QueryListRequest
{
}

public class GetGrandesUsuariosRequestValidator : Validator<GetGrandesUsuariosRequest>
{
    public GetGrandesUsuariosRequestValidator()
    {
        Include(new QueryListRequestValidator());
    }
}

/// <summary>
/// Response para retornar os bairros.
/// </summary>
public record GetGrandesUsuariosResponse
{
    /// <summary>
    /// Chave do grande usuário
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome do grande usuário
    /// </summary>
    public string Nome { get; set; } = default!;

    /// <summary>
    /// Abreviatura do nome do grande usuário (opcional)
    /// </summary>
    public string? NomeAbreviado { get; set; }

    /// <summary>
    /// Endereço do grande usuário
    /// </summary>
    public string Endereco { get; set; } = default!;

    /// <summary>
    /// CEP do grande usuário
    /// </summary>
    public string Cep { get; set; } = default!;

    /// <summary>
    /// Nome da localidade
    /// </summary>
    public string Localidade { get; set; } = default!;

    /// <summary>
    /// Sigla da UF
    /// </summary>
    public string Uf { get; set; } = default!;

    /// <summary>
    /// Nome do bairro
    /// </summary>
    public string Bairro { get; set; } = default!;

    /// <summary>
    /// Nome do logradouro
    /// </summary>
    public string? Logradouro { get; set; }

    /// <summary>
    /// Complemento do endereço
    /// </summary>
    public string? Complemento { get; set; }

    /// <summary>
    /// chave da localidade
    /// </summary>
    public int LocalidadeId { get; set; }

    /// <summary>
    /// chave do bairro
    /// </summary>
    public int BairroId { get; set; }

    /// <summary>
    /// chave do logradouro
    /// </summary>
    public int? LogradouroId { get; set; }
};

class GetGrandesUsuariosRequestMapper : GridifyMapper<GrandeUsuario>
{
    public GetGrandesUsuariosRequestMapper()
    {
        ClearMappings();

        AddMap(nameof(GetGrandesUsuariosResponse.Id), o => o.Id);
        AddMap(nameof(GetGrandesUsuariosResponse.Nome), o => o.Nome);
        AddMap(nameof(GetGrandesUsuariosResponse.NomeAbreviado), o => o.NomeAbreviado);
        AddMap(nameof(GetGrandesUsuariosResponse.Endereco), o => o.Endereco);
        AddMap(nameof(GetGrandesUsuariosResponse.Cep), o => o.Cep);
        AddMap(nameof(GetGrandesUsuariosResponse.Localidade), o => o.Localidade.Nome);
        AddMap(nameof(GetGrandesUsuariosResponse.Uf), o => o.Uf);
        AddMap(nameof(GetGrandesUsuariosResponse.Bairro), o => o.Bairro.Nome);
        AddMap(nameof(GetGrandesUsuariosResponse.Logradouro), o => o.Logradouro!.Nome);
        AddMap(nameof(GetGrandesUsuariosResponse.Complemento), o => o.Logradouro!.Complemento);

        AddMap(nameof(GetGrandesUsuariosResponse.LocalidadeId), o => o.LocalidadeId);
        AddMap(nameof(GetGrandesUsuariosResponse.BairroId), o => o.BairroId);
        AddMap(nameof(GetGrandesUsuariosResponse.LogradouroId), o => o.LogradouroId);
    }
}
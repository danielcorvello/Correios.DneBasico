using Correios.DneBasico.Api.Features.Logradouros;

namespace Correios.DneBasico.Api.IntegrationTests.Features.Logradouros;

[Collection<SutCollection>]
public class GetLogradourosEndpointTests(Sut sut) : TestBase
{
    [Fact(DisplayName = "Deve retornar os logradouros paginados (FastEndpoints)")]
    [Trait("Integração", nameof(GetLogradourosEndpoint))]
    public async Task Deve_Retornar_Logradouros_Paginados_FastEndpoints()
    {
        var request = new GetLogradourosRequest
        {
            PageNumber = 1,
            PageSize = 10
        };

        var (response, logradouros) = await sut.Client
            .GETAsync<GetLogradourosEndpoint, GetLogradourosRequest, PagedResponse<GetLogradourosResponse>>(request);

        response.EnsureSuccessStatusCode();

        logradouros.ShouldNotBeNull();
        logradouros.Data.Count.ShouldBeGreaterThan(0);
        logradouros.Data.Count.ShouldBeLessThanOrEqualTo(10);
        logradouros.Data.ShouldNotBeNull();
        logradouros.TotalCount.ShouldBeGreaterThan(0);
        logradouros.TotalPages.ShouldBeGreaterThan(0);
        logradouros.PageNumber.ShouldBe(1);
        logradouros.PageSize.ShouldBe(10);
    }

    [Fact(DisplayName = "Deve retornar lista ordenada de logradouros por cep (FastEndpoints)")]
    [Trait("Integração", nameof(GetLogradourosEndpoint))]
    public async Task Deve_Retornar_Lista_Ordenada_De_Logradouros_Por_Cep_FastEndpoints()
    {
        var request = new GetLogradourosRequest
        {
            OrderBy = "Cep asc, Id asc",
            PageNumber = 1,
            PageSize = 50
        };

        var (response, logradouros) = await sut.Client
            .GETAsync<GetLogradourosEndpoint, GetLogradourosRequest, PagedResponse<GetLogradourosResponse>>(request);

        response.EnsureSuccessStatusCode();
        logradouros.ShouldNotBeNull();
        logradouros.Data.Count.ShouldBeGreaterThan(0);

        var logradourosOrdenados = logradouros.Data
            .OrderBy(l => l.Cep)
            .ThenBy(l => l.Id)
            .ToList();
        logradouros.Data.ShouldBe(logradourosOrdenados);
    }

    [Fact(DisplayName = "Deve retornar logradouros de acordo com o filtro de UF (FastEndpoints)")]
    [Trait("Integração", nameof(GetLogradourosEndpoint))]
    public async Task Deve_Retornar_Logradouros_De_Acordo_Com_O_Filtro_De_Uf_FastEndpoints()
    {
        var request = new GetLogradourosRequest
        {
            Filter = "Uf=SP"
        };

        var (response, logradouros) = await sut.Client
            .GETAsync<GetLogradourosEndpoint, GetLogradourosRequest, PagedResponse<GetLogradourosResponse>>(request);

        response.EnsureSuccessStatusCode();
        logradouros.ShouldNotBeNull();
        logradouros.Data.Count.ShouldBeGreaterThan(0);
        logradouros.Data.ShouldAllBe(l => l.Uf.Equals("SP", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Deve retornar logradouros de acordo com o filtro de nome (FastEndpoints)")]
    [Trait("Integração", nameof(GetLogradourosEndpoint))]
    public async Task Deve_Retornar_Logradouros_De_Acordo_Com_O_Filtro_De_Nome_FastEndpoints()
    {
        var request = new GetLogradourosRequest
        {
            Filter = "Nome=*Centro"
        };

        var (response, logradouros) = await sut.Client
            .GETAsync<GetLogradourosEndpoint, GetLogradourosRequest, PagedResponse<GetLogradourosResponse>>(request);

        response.EnsureSuccessStatusCode();
        logradouros.ShouldNotBeNull();
        logradouros.Data.Count.ShouldBeGreaterThan(0);
        logradouros.Data.ShouldAllBe(l => l.Nome.Contains("Centro", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para filtro inválido (FastEndpoints)")]
    [Trait("Integração", nameof(GetLogradourosEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Filtro_Invalido_FastEndpoints()
    {
        var request = new GetLogradourosRequest
        {
            Filter = "PropriedadeInexistente=0"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetLogradourosEndpoint, GetLogradourosRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para paginação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetLogradourosEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Paginacao_Invalida_FastEndpoints()
    {
        var request = new GetLogradourosRequest
        {
            PageNumber = 0,
            PageSize = -5
        };

        var (response, res) = await sut.Client
            .GETAsync<GetLogradourosEndpoint, GetLogradourosRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldNotBeNull();
        res.Errors.Count.ShouldBe(2);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_NUMBER_QUERY);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_SIZE_QUERY);
    }

    [Fact(DisplayName = "Deve retornar erro 400 para ordenação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetLogradourosEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Ordenacao_Invalida_FastEndpoints()
    {
        var request = new GetLogradourosRequest
        {
            OrderBy = "PropriedadeInexistente asc"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetLogradourosEndpoint, GetLogradourosRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }
}

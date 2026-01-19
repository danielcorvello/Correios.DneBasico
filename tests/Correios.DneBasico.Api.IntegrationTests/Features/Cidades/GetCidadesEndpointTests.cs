using Correios.DneBasico.Api.Features.Cidades;

namespace Correios.DneBasico.Api.IntegrationTests.Features.Cidades;

[Collection<SutCollection>]
public class GetCidadesEndpointTests(Sut sut) : TestBase
{
    [Fact(DisplayName = "Deve retornar as cidades paginadas (FastEndpoints)")]
    [Trait("Integração", nameof(GetCidadesEndpoint))]
    public async Task Deve_Retornar_Cidades_Paginadas_FastEndpoints()
    {
        var request = new GetCidadesRequest
        {
            Uf = "SP",
            PageNumber = 1,
            PageSize = 10
        };

        var (response, cidades) = await sut.Client
            .GETAsync<GetCidadesEndpoint, GetCidadesRequest, PagedResponse<GetCidadesResponse>>(request);

        response.EnsureSuccessStatusCode();

        cidades.ShouldNotBeNull();
        cidades.Data.Count.ShouldBeGreaterThan(0);
        cidades.Data.Count.ShouldBeLessThanOrEqualTo(10);
        cidades.Data.ShouldNotBeNull();
        cidades.TotalCount.ShouldBeGreaterThan(0);
        cidades.TotalPages.ShouldBeGreaterThan(0);
        cidades.PageNumber.ShouldBe(1);
        cidades.PageSize.ShouldBe(10);
        cidades.Data.ShouldAllBe(c => c.Uf.Equals("SP", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Deve retornar lista ordenada de cidades por ibge (FastEndpoints)")]
    [Trait("Integração", nameof(GetCidadesEndpoint))]
    public async Task Deve_Retornar_Lista_Ordenada_De_Cidades_Por_Ibge_FastEndpoints()
    {
        var request = new GetCidadesRequest
        {
            Uf = "SP",
            OrderBy = "Ibge asc, Id asc",
            PageNumber = 1,
            PageSize = 50
        };

        var (response, cidades) = await sut.Client
            .GETAsync<GetCidadesEndpoint, GetCidadesRequest, PagedResponse<GetCidadesResponse>>(request);

        response.EnsureSuccessStatusCode();
        cidades.ShouldNotBeNull();
        cidades.Data.Count.ShouldBeGreaterThan(0);

        var cidadesOrdenadas = cidades.Data
            .OrderBy(c => c.Ibge)
            .ThenBy(c => c.Id)
            .ToList();
        cidades.Data.ShouldBe(cidadesOrdenadas);
    }

    [Fact(DisplayName = "Deve retornar cidades de acordo com o filtro de nome (FastEndpoints)")]
    [Trait("Integração", nameof(GetCidadesEndpoint))]
    public async Task Deve_Retornar_Cidades_De_Acordo_Com_O_Filtro_De_Nome_FastEndpoints()
    {
        var request = new GetCidadesRequest
        {
            Uf = "SP",
            Filter = "Nome=*Santos"
        };

        var (response, cidades) = await sut.Client
            .GETAsync<GetCidadesEndpoint, GetCidadesRequest, PagedResponse<GetCidadesResponse>>(request);

        response.EnsureSuccessStatusCode();
        cidades.ShouldNotBeNull();
        cidades.Data.Count.ShouldBeGreaterThan(0);
        cidades.Data.ShouldAllBe(c => c.Nome.Contains("Santos", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para UF inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetCidadesEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Uf_Invalida_FastEndpoints()
    {
        var request = new GetCidadesRequest
        {
            Uf = "S", // menos de 2 caracteres
            PageNumber = 1,
            PageSize = 10
        };

        var (response, problem) = await sut.Client
            .GETAsync<GetCidadesEndpoint, GetCidadesRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        problem.ShouldNotBeNull();

    }

    [Fact(DisplayName = "Deve retornar erro 400 para filtro inválido (FastEndpoints)")]
    [Trait("Integração", nameof(GetCidadesEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Filtro_Invalido_FastEndpoints()
    {
        var request = new GetCidadesRequest
        {
            Uf = "SP",
            Filter = "PropriedadeInexistente=0"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetCidadesEndpoint, GetCidadesRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para paginação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetCidadesEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Paginacao_Invalida_FastEndpoints()
    {
        var request = new GetCidadesRequest
        {
            Uf = "SP",
            PageNumber = 0,
            PageSize = -5
        };

        var (response, res) = await sut.Client
            .GETAsync<GetCidadesEndpoint, GetCidadesRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldNotBeNull();
        res.Errors.Count.ShouldBe(2);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_NUMBER_QUERY);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_SIZE_QUERY);
    }

    [Fact(DisplayName = "Deve retornar erro 400 para ordenação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetCidadesEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Ordenacao_Invalida_FastEndpoints()
    {
        var request = new GetCidadesRequest
        {
            Uf = "SP",
            OrderBy = "PropriedadeInexistente asc"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetCidadesEndpoint, GetCidadesRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }
}

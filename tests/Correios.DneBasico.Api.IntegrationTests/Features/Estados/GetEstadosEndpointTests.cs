using Correios.DneBasico.Api.Features.Estados;

namespace Correios.DneBasico.Api.IntegrationTests.Features.Estados;

[Collection<SutCollection>]
public class GetEstadosEndpointTests(Sut sut) : TestBase
{
    [Fact(DisplayName = "Deve retornar os estados paginados (FastEndpoints)")]
    [Trait("Integração", nameof(GetEstadosEndpoint))]
    public async Task Deve_Retornar_Estados_Paginados_FastEndpoints()
    {
        var pageNumber = 1;
        var pageSize = 10;

        var request = new GetEstadosRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var (response, estados) = await sut.Client
            .GETAsync<GetEstadosEndpoint, GetEstadosRequest, PagedResponse<GetEstadosResponse>>(request);

        response.EnsureSuccessStatusCode();

        estados.ShouldNotBeNull();
        estados.Data.Count.ShouldBeGreaterThan(0);
        estados.Data.Count.ShouldBeLessThanOrEqualTo(pageSize);
        estados.Data.ShouldNotBeNull();
        estados.TotalCount.ShouldBeGreaterThan(0);
        estados.TotalPages.ShouldBeGreaterThan(0);
        estados.PageNumber.ShouldBe(pageNumber);
        estados.PageSize.ShouldBe(pageSize);
    }

    [Fact(DisplayName = "Deve retornar lista ordenada de estados por uf")]
    [Trait("Integração", nameof(GetEstadosEndpoint))]
    public async Task Deve_Retornar_Lista_Ordenada_De_Estados_Por_Uf()
    {
        var request = new GetEstadosRequest
        {
            OrderBy = "Uf asc",
            PageNumber = 1,
            PageSize = 50
        };

        var (response, estados) = await sut.Client
            .GETAsync<GetEstadosEndpoint, GetEstadosRequest, PagedResponse<GetEstadosResponse>>(request);

        response.EnsureSuccessStatusCode();
        estados.ShouldNotBeNull();
        estados.Data.Count.ShouldBeGreaterThan(0);

        var estadosOrdenados = estados.Data.OrderBy(e => e.Uf).ToList();
        estados.Data.ShouldBeEquivalentTo(estadosOrdenados);
    }

    [Fact(DisplayName = "Deve retornar estados de acordo com o filtro de UF")]
    [Trait("Integração", nameof(GetEstadosEndpoint))]
    public async Task Deve_Retornar_Estados_De_Acordo_Com_O_Filtro_De_Uf()
    {
        var filter = "Uf=SP";

        var request = new GetEstadosRequest
        {
            Filter = filter
        };

        var (response, estados) = await sut.Client
            .GETAsync<GetEstadosEndpoint, GetEstadosRequest, PagedResponse<GetEstadosResponse>>(request);

        response.EnsureSuccessStatusCode();
        estados.ShouldNotBeNull();
        estados.Data.Count.ShouldBeGreaterThan(0);
        estados.Data.ShouldAllBe(e => e.Uf == "SP");
    }

    [Fact(DisplayName = "Deve retornar erro 400 para filtro inválido (FastEndpoints)")]
    [Trait("Integração", nameof(GetEstadosEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Filtro_Invalido_FastEndpoints()
    {
        var request = new GetEstadosRequest
        {
            Filter = "PropriedadeInexistente=0"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetEstadosEndpoint, GetEstadosRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para paginação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetEstadosEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Paginacao_Invalida_FastEndpoints()
    {
        var request = new GetEstadosRequest
        {
            PageNumber = 0,
            PageSize = -5
        };

        var (response, res) = await sut.Client
            .GETAsync<GetEstadosEndpoint, GetEstadosRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldNotBeNull();
        res.Errors.Count.ShouldBe(2);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_NUMBER_QUERY);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_SIZE_QUERY);
    }

    [Fact(DisplayName = "Deve retornar erro 400 para ordenação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetEstadosEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Ordenacao_Invalida_FastEndpoints()
    {
        var request = new GetEstadosRequest
        {
            OrderBy = "PropriedadeInexistente asc"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetEstadosEndpoint, GetEstadosRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }
}

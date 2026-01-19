using Correios.DneBasico.Api.Features.Ceps;

namespace Correios.DneBasico.Api.IntegrationTests.Features.Ceps;

[Collection<SutCollection>]
public class GetCepsEndpointTests(Sut sut) : TestBase
{
    [Fact(DisplayName = "Deve retornar os ceps paginados (FastEndpoints)")]
    [Trait("Integração", nameof(GetCepsEndpoint))]
    public async Task Deve_Retornar_Ceps_Paginados_FastEndpoints()
    {
        var pageNumber = 1;
        var pageSize = 10;

        var request = new GetCepsRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var (response, ceps) = await sut.Client
            .GETAsync<GetCepsEndpoint, GetCepsRequest, PagedResponse<GetCepsResponse>>(request);

        response.EnsureSuccessStatusCode();

        ceps.ShouldNotBeNull();
        ceps.Data.Count.ShouldBeGreaterThan(0);
        ceps.Data.Count.ShouldBeLessThanOrEqualTo(pageSize);
        ceps.Data.ShouldNotBeNull();
        ceps.TotalCount.ShouldBeGreaterThan(0);
        ceps.TotalPages.ShouldBeGreaterThan(0);
        ceps.PageNumber.ShouldBe(pageNumber);
        ceps.PageSize.ShouldBe(pageSize);
    }

    [Fact(DisplayName = "Deve retornar lista ordenada de ceps por código")]
    [Trait("Integração", nameof(GetCepsEndpoint))]
    public async Task Deve_Retornar_Lista_Ordenada_De_Ceps_Por_Codigo()
    {
        var request = new GetCepsRequest
        {
            OrderBy = "Codigo asc",
            PageNumber = 1,
            PageSize = 50
        };

        var (response, ceps) = await sut.Client
            .GETAsync<GetCepsEndpoint, GetCepsRequest, PagedResponse<GetCepsResponse>>(request);

        response.EnsureSuccessStatusCode();
        ceps.ShouldNotBeNull();
        ceps.Data.Count.ShouldBeGreaterThan(0);

        var cepsOrdenados = ceps.Data.OrderBy(c => c.Codigo).ToList();
        ceps.Data.ShouldBe(cepsOrdenados);
    }

    [Fact(DisplayName = "Deve retornar ceps de acordo com o filtro de Ibge")]
    [Trait("Integração", nameof(GetCepsEndpoint))]
    public async Task Deve_Retornar_Ceps_De_Acordo_Com_O_Filtro_De_Municipio()
    {
        var filter = "Ibge=3552502";

        var request = new GetCepsRequest
        {
            Filter = filter
        };

        var (response, ceps) = await sut.Client
            .GETAsync<GetCepsEndpoint, GetCepsRequest, PagedResponse<GetCepsResponse>>(request);

        response.EnsureSuccessStatusCode();
        ceps.ShouldNotBeNull();
        ceps.Data.Count.ShouldBeGreaterThan(0);
        ceps.Data.ShouldAllBe(c => c.Ibge == "3552502");
    }

    [Fact(DisplayName = "Deve retornar erro 400 para filtro inválido (FastEndpoints)")]
    [Trait("Integração", nameof(GetCepsEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Filtro_Invalido_FastEndpoints()
    {
        var request = new GetCepsRequest
        {
            Filter = "PropriedadeInexistente=0"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetCepsEndpoint, GetCepsRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para paginação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetCepsEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Paginacao_Invalida_FastEndpoints()
    {
        var request = new GetCepsRequest
        {
            PageNumber = 0,
            PageSize = -5
        };

        var (response, res) = await sut.Client
            .GETAsync<GetCepsEndpoint, GetCepsRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldNotBeNull();
        res.Errors.Count.ShouldBe(2);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_NUMBER_QUERY);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_SIZE_QUERY);
    }

    [Fact(DisplayName = "Deve retornar erro 400 para ordenação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetCepsEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Ordenacao_Invalida_FastEndpoints()
    {
        var request = new GetCepsRequest
        {
            OrderBy = "PropriedadeInexistente asc"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetCepsEndpoint, GetCepsRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }
}

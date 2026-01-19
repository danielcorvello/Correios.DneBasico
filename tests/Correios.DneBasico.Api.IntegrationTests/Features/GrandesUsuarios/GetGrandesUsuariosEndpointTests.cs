using Correios.DneBasico.Api.Features.GrandesUsuarios;

namespace Correios.DneBasico.Api.IntegrationTests.Features.GrandesUsuarios;

[Collection<SutCollection>]
public class GetGrandesUsuariosEndpointTests(Sut sut) : TestBase
{
    [Fact(DisplayName = "Deve retornar grandes usuários paginados (FastEndpoints)")]
    [Trait("Integração", nameof(GetGrandesUsuariosEndpoint))]
    public async Task Deve_Retornar_Grandes_Usuarios_Paginados_FastEndpoints()
    {
        var pageNumber = 1;
        var pageSize = 10;

        var request = new GetGrandesUsuariosRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var (response, grandesUsuarios) = await sut.Client
            .GETAsync<GetGrandesUsuariosEndpoint, GetGrandesUsuariosRequest, PagedResponse<GetGrandesUsuariosResponse>>(request);

        response.EnsureSuccessStatusCode();

        grandesUsuarios.ShouldNotBeNull();
        grandesUsuarios.Data.Count.ShouldBeGreaterThan(0);
        grandesUsuarios.Data.Count.ShouldBeLessThanOrEqualTo(pageSize);
        grandesUsuarios.Data.ShouldNotBeNull();
        grandesUsuarios.TotalCount.ShouldBeGreaterThan(0);
        grandesUsuarios.TotalPages.ShouldBeGreaterThan(0);
        grandesUsuarios.PageNumber.ShouldBe(pageNumber);
        grandesUsuarios.PageSize.ShouldBe(pageSize);
    }

    [Fact(DisplayName = "Deve retornar lista ordenada de grandes usuários por cep")]
    [Trait("Integração", nameof(GetGrandesUsuariosEndpoint))]
    public async Task Deve_Retornar_Lista_Ordenada_De_Grandes_Usuarios_Por_Cep()
    {
        var request = new GetGrandesUsuariosRequest
        {
            OrderBy = "Cep asc, Id asc",
            PageNumber = 1,
            PageSize = 50
        };

        var (response, grandesUsuarios) = await sut.Client
            .GETAsync<GetGrandesUsuariosEndpoint, GetGrandesUsuariosRequest, PagedResponse<GetGrandesUsuariosResponse>>(request);

        response.EnsureSuccessStatusCode();
        grandesUsuarios.ShouldNotBeNull();
        grandesUsuarios.Data.Count.ShouldBeGreaterThan(0);

        var grandesUsuariosOrdenados = grandesUsuarios.Data
            .OrderBy(g => g.Cep)
            .ThenBy(g => g.Id)
            .ToList();

        grandesUsuarios.Data.ShouldBe(grandesUsuariosOrdenados);
    }

    [Fact(DisplayName = "Deve retornar grandes usuários de acordo com o filtro de UF (FastEndpoints)")]
    [Trait("Integração", nameof(GetGrandesUsuariosEndpoint))]
    public async Task Deve_Retornar_Grandes_Usuarios_De_Acordo_Com_O_Filtro_De_Uf_FastEndpoints()
    {
        var filter = "Uf=SP";

        var request = new GetGrandesUsuariosRequest
        {
            Filter = filter
        };

        var (response, grandesUsuarios) = await sut.Client
            .GETAsync<GetGrandesUsuariosEndpoint, GetGrandesUsuariosRequest, PagedResponse<GetGrandesUsuariosResponse>>(request);

        response.EnsureSuccessStatusCode();
        grandesUsuarios.ShouldNotBeNull();
        grandesUsuarios.Data.Count.ShouldBeGreaterThan(0);
        grandesUsuarios.Data.ShouldAllBe(g => g.Uf == "SP");
    }

    [Fact(DisplayName = "Deve retornar erro 400 para filtro inválido (FastEndpoints)")]
    [Trait("Integração", nameof(GetGrandesUsuariosEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Filtro_Invalido_FastEndpoints()
    {
        var request = new GetGrandesUsuariosRequest
        {
            Filter = "PropriedadeInexistente=0"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetGrandesUsuariosEndpoint, GetGrandesUsuariosRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para paginação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetGrandesUsuariosEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Paginacao_Invalida_FastEndpoints()
    {
        var request = new GetGrandesUsuariosRequest
        {
            PageNumber = 0,
            PageSize = -5
        };

        var (response, res) = await sut.Client
            .GETAsync<GetGrandesUsuariosEndpoint, GetGrandesUsuariosRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldNotBeNull();
        res.Errors.Count.ShouldBe(2);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_NUMBER_QUERY);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_SIZE_QUERY);
    }

    [Fact(DisplayName = "Deve retornar erro 400 para ordenação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetGrandesUsuariosEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Ordenacao_Invalida_FastEndpoints()
    {
        var request = new GetGrandesUsuariosRequest
        {
            OrderBy = "PropriedadeInexistente asc"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetGrandesUsuariosEndpoint, GetGrandesUsuariosRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }
}

using Correios.DneBasico.Api.Features.Localidades;

namespace Correios.DneBasico.Api.IntegrationTests.Features.Localidades;

[Collection<SutCollection>]
public class GetLocalidadesEndpointTests(Sut sut) : TestBase
{
    [Fact(DisplayName = "Deve retornar as localidades paginadas (FastEndpoints)")]
    [Trait("Integração", nameof(GetLocalidadesEndpoint))]
    public async Task Deve_Retornar_Localidades_Paginadas_FastEndpoints()
    {
        var request = new GetLocalidadesRequest
        {
            PageNumber = 1,
            PageSize = 10
        };

        var (response, localidades) = await sut.Client
            .GETAsync<GetLocalidadesEndpoint, GetLocalidadesRequest, PagedResponse<GetLocalidadesResponse>>(request);

        response.EnsureSuccessStatusCode();

        localidades.ShouldNotBeNull();
        localidades.Data.Count.ShouldBeGreaterThan(0);
        localidades.Data.Count.ShouldBeLessThanOrEqualTo(10);
        localidades.Data.ShouldNotBeNull();
        localidades.TotalCount.ShouldBeGreaterThan(0);
        localidades.TotalPages.ShouldBeGreaterThan(0);
        localidades.PageNumber.ShouldBe(1);
        localidades.PageSize.ShouldBe(10);
    }

    [Fact(DisplayName = "Deve retornar lista ordenada de localidades por CEP (FastEndpoints)")]
    [Trait("Integração", nameof(GetLocalidadesEndpoint))]
    public async Task Deve_Retornar_Lista_Ordenada_De_Localidades_Por_Cep_FastEndpoints()
    {
        var request = new GetLocalidadesRequest
        {
            Filter = "Cep!=",
            OrderBy = "Cep asc, Id asc",
            PageNumber = 1,
            PageSize = 50
        };

        var (response, localidades) = await sut.Client
            .GETAsync<GetLocalidadesEndpoint, GetLocalidadesRequest, PagedResponse<GetLocalidadesResponse>>(request);

        response.EnsureSuccessStatusCode();
        localidades.ShouldNotBeNull();
        localidades.Data.Count.ShouldBeGreaterThan(0);

        var localidadesOrdenadas = localidades.Data
            .OrderBy(l => l.Cep)
            .ThenBy(l => l.Id)
            .ToList();
        localidades.Data.ShouldBe(localidadesOrdenadas);
    }

    [Fact(DisplayName = "Deve retornar localidades de acordo com o filtro de UF (FastEndpoints)")]
    [Trait("Integração", nameof(GetLocalidadesEndpoint))]
    public async Task Deve_Retornar_Localidades_De_Acordo_Com_O_Filtro_De_Uf_FastEndpoints()
    {
        var request = new GetLocalidadesRequest
        {
            Filter = "Uf=SP"
        };

        var (response, localidades) = await sut.Client
            .GETAsync<GetLocalidadesEndpoint, GetLocalidadesRequest, PagedResponse<GetLocalidadesResponse>>(request);

        response.EnsureSuccessStatusCode();
        localidades.ShouldNotBeNull();
        localidades.Data.Count.ShouldBeGreaterThan(0);
        localidades.Data.ShouldAllBe(l => l.Uf.Equals("SP", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Deve retornar localidades de acordo com o filtro de nome (FastEndpoints)")]
    [Trait("Integração", nameof(GetLocalidadesEndpoint))]
    public async Task Deve_Retornar_Localidades_De_Acordo_Com_O_Filtro_De_Nome_FastEndpoints()
    {
        var request = new GetLocalidadesRequest
        {
            Filter = "Nome=*São paulo"
        };

        var (response, localidades) = await sut.Client
            .GETAsync<GetLocalidadesEndpoint, GetLocalidadesRequest, PagedResponse<GetLocalidadesResponse>>(request);

        response.EnsureSuccessStatusCode();
        localidades.ShouldNotBeNull();
        localidades.Data.Count.ShouldBeGreaterThan(0);
        localidades.Data.ShouldAllBe(l => l.Nome.Contains("São paulo", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para filtro inválido (FastEndpoints)")]
    [Trait("Integração", nameof(GetLocalidadesEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Filtro_Invalido_FastEndpoints()
    {
        var request = new GetLocalidadesRequest
        {
            Filter = "PropriedadeInexistente=0"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetLocalidadesEndpoint, GetLocalidadesRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para paginação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetLocalidadesEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Paginacao_Invalida_FastEndpoints()
    {
        var request = new GetLocalidadesRequest
        {
            PageNumber = 0,
            PageSize = -5
        };

        var (response, res) = await sut.Client
            .GETAsync<GetLocalidadesEndpoint, GetLocalidadesRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldNotBeNull();
        res.Errors.Count.ShouldBe(2);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_NUMBER_QUERY);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_SIZE_QUERY);
    }

    [Fact(DisplayName = "Deve retornar erro 400 para ordenação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetLocalidadesEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Ordenacao_Invalida_FastEndpoints()
    {
        var request = new GetLocalidadesRequest
        {
            OrderBy = "PropriedadeInexistente asc"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetLocalidadesEndpoint, GetLocalidadesRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }
}

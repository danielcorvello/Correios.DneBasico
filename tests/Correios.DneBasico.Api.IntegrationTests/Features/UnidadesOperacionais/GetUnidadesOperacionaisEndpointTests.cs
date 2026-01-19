using Correios.DneBasico.Api.Features.UnidadesOperacionais;

namespace Correios.DneBasico.Api.IntegrationTests.Features.UnidadesOperacionais;

[Collection<SutCollection>]
public class GetUnidadesOperacionaisEndpointTests(Sut sut) : TestBase
{
    [Fact(DisplayName = "Deve retornar as unidades operacionais paginadas (FastEndpoints)")]
    [Trait("Integração", nameof(GetUnidadesOperacionaisEndpoint))]
    public async Task Deve_Retornar_Unidades_Operacionais_Paginadas_FastEndpoints()
    {
        var request = new GetUnidadesOperacionaisRequest
        {
            PageNumber = 1,
            PageSize = 10
        };

        var (response, unidades) = await sut.Client
            .GETAsync<GetUnidadesOperacionaisEndpoint, GetUnidadesOperacionaisRequest, PagedResponse<GetUnidadesOperacionaisResponse>>(request);

        response.EnsureSuccessStatusCode();

        unidades.ShouldNotBeNull();
        unidades.Data.Count.ShouldBeGreaterThan(0);
        unidades.Data.Count.ShouldBeLessThanOrEqualTo(10);
        unidades.Data.ShouldNotBeNull();
        unidades.TotalCount.ShouldBeGreaterThan(0);
        unidades.TotalPages.ShouldBeGreaterThan(0);
        unidades.PageNumber.ShouldBe(1);
        unidades.PageSize.ShouldBe(10);
    }

    [Fact(DisplayName = "Deve retornar lista ordenada de unidades operacionais por cep (FastEndpoints)")]
    [Trait("Integração", nameof(GetUnidadesOperacionaisEndpoint))]
    public async Task Deve_Retornar_Lista_Ordenada_De_Unidades_Operacionais_Por_Cep_FastEndpoints()
    {
        var request = new GetUnidadesOperacionaisRequest
        {
            OrderBy = "Cep asc, Id asc",
            PageNumber = 1,
            PageSize = 50
        };

        var (response, unidades) = await sut.Client
            .GETAsync<GetUnidadesOperacionaisEndpoint, GetUnidadesOperacionaisRequest, PagedResponse<GetUnidadesOperacionaisResponse>>(request);

        response.EnsureSuccessStatusCode();
        unidades.ShouldNotBeNull();
        unidades.Data.Count.ShouldBeGreaterThan(0);

        var unidadesOrdenadas = unidades.Data
            .OrderBy(u => u.Cep)
            .ThenBy(u => u.Id)
            .ToList();
        unidades.Data.ShouldBe(unidadesOrdenadas);
    }

    [Fact(DisplayName = "Deve retornar unidades operacionais de acordo com o filtro de UF (FastEndpoints)")]
    [Trait("Integração", nameof(GetUnidadesOperacionaisEndpoint))]
    public async Task Deve_Retornar_Unidades_Operacionais_De_Acordo_Com_O_Filtro_De_Uf_FastEndpoints()
    {
        var request = new GetUnidadesOperacionaisRequest
        {
            Filter = "Uf=SP"
        };

        var (response, unidades) = await sut.Client
            .GETAsync<GetUnidadesOperacionaisEndpoint, GetUnidadesOperacionaisRequest, PagedResponse<GetUnidadesOperacionaisResponse>>(request);

        response.EnsureSuccessStatusCode();
        unidades.ShouldNotBeNull();
        unidades.Data.Count.ShouldBeGreaterThan(0);
        unidades.Data.ShouldAllBe(u => u.Uf.Equals("SP", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Deve retornar unidades operacionais de acordo com o filtro de nome (FastEndpoints)")]
    [Trait("Integração", nameof(GetUnidadesOperacionaisEndpoint))]
    public async Task Deve_Retornar_Unidades_Operacionais_De_Acordo_Com_O_Filtro_De_Nome_FastEndpoints()
    {
        var request = new GetUnidadesOperacionaisRequest
        {
            Filter = "Nome=*Central"
        };

        var (response, unidades) = await sut.Client
            .GETAsync<GetUnidadesOperacionaisEndpoint, GetUnidadesOperacionaisRequest, PagedResponse<GetUnidadesOperacionaisResponse>>(request);

        response.EnsureSuccessStatusCode();
        unidades.ShouldNotBeNull();
        unidades.Data.Count.ShouldBeGreaterThan(0);
        unidades.Data.ShouldAllBe(u => u.Nome.Contains("Central", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para filtro inválido (FastEndpoints)")]
    [Trait("Integração", nameof(GetUnidadesOperacionaisEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Filtro_Invalido_FastEndpoints()
    {
        var request = new GetUnidadesOperacionaisRequest
        {
            Filter = "PropriedadeInexistente=0"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetUnidadesOperacionaisEndpoint, GetUnidadesOperacionaisRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para paginação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetUnidadesOperacionaisEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Paginacao_Invalida_FastEndpoints()
    {
        var request = new GetUnidadesOperacionaisRequest
        {
            PageNumber = 0,
            PageSize = -5
        };

        var (response, res) = await sut.Client
            .GETAsync<GetUnidadesOperacionaisEndpoint, GetUnidadesOperacionaisRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldNotBeNull();
        res.Errors.Count.ShouldBe(2);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_NUMBER_QUERY);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_SIZE_QUERY);
    }

    [Fact(DisplayName = "Deve retornar erro 400 para ordenação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetUnidadesOperacionaisEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Ordenacao_Invalida_FastEndpoints()
    {
        var request = new GetUnidadesOperacionaisRequest
        {
            OrderBy = "PropriedadeInexistente asc"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetUnidadesOperacionaisEndpoint, GetUnidadesOperacionaisRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }
}

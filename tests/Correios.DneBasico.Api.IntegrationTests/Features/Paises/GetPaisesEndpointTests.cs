using Correios.DneBasico.Api.Features.Paises;

namespace Correios.DneBasico.Api.IntegrationTests.Features.Paises;

[Collection<SutCollection>]
public class GetPaisesEndpointTests(Sut sut) : TestBase
{
    [Fact(DisplayName = "Deve retornar os países paginados (FastEndpoints)")]
    [Trait("Integração", nameof(GetPaisesEndpoint))]
    public async Task Deve_Retornar_Paises_Paginados_FastEndpoints()
    {
        var request = new GetPaisesRequest
        {
            PageNumber = 1,
            PageSize = 10
        };

        var (response, paises) = await sut.Client
            .GETAsync<GetPaisesEndpoint, GetPaisesRequest, PagedResponse<GetPaisesResponse>>(request);

        response.EnsureSuccessStatusCode();

        paises.ShouldNotBeNull();
        paises.Data.Count.ShouldBeGreaterThan(0);
        paises.Data.Count.ShouldBeLessThanOrEqualTo(10);
        paises.Data.ShouldNotBeNull();
        paises.TotalCount.ShouldBeGreaterThan(0);
        paises.TotalPages.ShouldBeGreaterThan(0);
        paises.PageNumber.ShouldBe(1);
        paises.PageSize.ShouldBe(10);
    }

    [Fact(DisplayName = "Deve retornar lista ordenada de países por sigla (FastEndpoints)")]
    [Trait("Integração", nameof(GetPaisesEndpoint))]
    public async Task Deve_Retornar_Lista_Ordenada_De_Paises_Por_Sigla_FastEndpoints()
    {
        var request = new GetPaisesRequest
        {
            OrderBy = "Sigla asc, NomePortugues asc",
            PageNumber = 1,
            PageSize = 50
        };

        var (response, paises) = await sut.Client
            .GETAsync<GetPaisesEndpoint, GetPaisesRequest, PagedResponse<GetPaisesResponse>>(request);

        response.EnsureSuccessStatusCode();
        paises.ShouldNotBeNull();
        paises.Data.Count.ShouldBeGreaterThan(0);

        var data = paises.Data;

        for (var i = 1; i < data.Count; i++)
        {
            var prev = data[i - 1];
            var curr = data[i];

            var siglaCmp = string.Compare(prev.Sigla, curr.Sigla, StringComparison.OrdinalIgnoreCase);

            if (siglaCmp == 0)
            {
                var nomeCmp = string.Compare(prev.NomePortugues, curr.NomePortugues, StringComparison.OrdinalIgnoreCase);
                nomeCmp.ShouldBeLessThanOrEqualTo(0);
            }
            else
            {
                siglaCmp.ShouldBeLessThanOrEqualTo(0);
            }
        }
    }

    [Fact(DisplayName = "Deve retornar países de acordo com o filtro de nome em português (FastEndpoints)")]
    [Trait("Integração", nameof(GetPaisesEndpoint))]
    public async Task Deve_Retornar_Paises_De_Acordo_Com_O_Filtro_De_Nome_Portugues_FastEndpoints()
    {
        var request = new GetPaisesRequest
        {
            Filter = "NomePortugues=*Brasil"
        };

        var (response, paises) = await sut.Client
            .GETAsync<GetPaisesEndpoint, GetPaisesRequest, PagedResponse<GetPaisesResponse>>(request);

        response.EnsureSuccessStatusCode();
        paises.ShouldNotBeNull();
        paises.Data.Count.ShouldBeGreaterThan(0);
        paises.Data.ShouldAllBe(p => p.NomePortugues.Contains("Brasil", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para filtro inválido (FastEndpoints)")]
    [Trait("Integração", nameof(GetPaisesEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Filtro_Invalido_FastEndpoints()
    {
        var request = new GetPaisesRequest
        {
            Filter = "PropriedadeInexistente=0"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetPaisesEndpoint, GetPaisesRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para paginação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetPaisesEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Paginacao_Invalida_FastEndpoints()
    {
        var request = new GetPaisesRequest
        {
            PageNumber = 0,
            PageSize = -5
        };

        var (response, res) = await sut.Client
            .GETAsync<GetPaisesEndpoint, GetPaisesRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldNotBeNull();
        res.Errors.Count.ShouldBe(2);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_NUMBER_QUERY);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_SIZE_QUERY);
    }

    [Fact(DisplayName = "Deve retornar erro 400 para ordenação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetPaisesEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Ordenacao_Invalida_FastEndpoints()
    {
        var request = new GetPaisesRequest
        {
            OrderBy = "PropriedadeInexistente asc"
        };

        var (response, res) = await sut.Client
            .GETAsync<GetPaisesEndpoint, GetPaisesRequest, ErrorResponse>(request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }
}

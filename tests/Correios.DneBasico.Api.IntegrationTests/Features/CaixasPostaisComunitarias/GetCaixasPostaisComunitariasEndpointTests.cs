using Correios.DneBasico.Api.Features.CaixasPostaisComunitarias;

namespace Correios.DneBasico.Api.IntegrationTests.Features.CaixasPostaisComunitarias;

[Collection<SutCollection>]
public class GetCaixasPostaisComunitariasEndpointTests(Sut sut) : TestBase
{
    [Fact(DisplayName = "Deve retornar as caixas postais comunitárias paginadas")]
    [Trait("Integração", nameof(GetCaixasPostaisComunitariasEndpoint))]
    public async Task Deve_Retornar_Caixas_Postais_Comunitarias_Paginadas_HttpClient()
    {
        var pageNumber = 1;
        var pageSize = 10;
        var url = new StringBuilder();
        url.Append($"{ApiConstants.RouteNames.CAIXAS_POSTAIS_COMUNITARIAS}?");
        url.Append($"{ApiConstants.RouteNames.PAGE_NUMBER_QUERY}={pageNumber}&");
        url.Append($"{ApiConstants.RouteNames.PAGE_SIZE_QUERY}={pageSize}");

        // Arrange & Act
        var request = new HttpRequestMessage(HttpMethod.Get, url.ToString());
        var response = await sut.Client.SendAsync(request, sut.Cancellation);

        // Assert
        response.EnsureSuccessStatusCode();

        var caixasPostais = await response.Content.ReadFromJsonAsync<PagedResponse<GetCaixasPostaisComunitariasResponse>>(cancellationToken: sut.Cancellation);
        caixasPostais.ShouldNotBeNull();
        caixasPostais.Data.Count.ShouldBeGreaterThan(0);
        caixasPostais.Data.Count.ShouldBeLessThanOrEqualTo(10);
        caixasPostais.Data.ShouldNotBeNull();
        caixasPostais.TotalCount.ShouldBeGreaterThan(0);
        caixasPostais.TotalPages.ShouldBeGreaterThan(0);
        caixasPostais.PageNumber.ShouldBe(pageNumber);
        caixasPostais.PageSize.ShouldBe(pageSize);
    }

    [Fact(DisplayName = "Deve retornar as caixas postais comunitárias paginadas (FastEndpoints)")]
    [Trait("Integração", nameof(GetCaixasPostaisComunitariasEndpoint))]
    public async Task Deve_Retornar_Caixas_Postais_Comunitarias_Paginadas_FastEndpoints()
    {
        var pageNumber = 1;
        var pageSize = 10;
        var request = new GetCaixasPostaisComunitariasRequest()
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var (response, caixasPostais) = await sut.Client.GETAsync<GetCaixasPostaisComunitariasEndpoint, GetCaixasPostaisComunitariasRequest, PagedResponse<GetCaixasPostaisComunitariasResponse>>(request);

        // Assert
        response.EnsureSuccessStatusCode();
        caixasPostais.ShouldNotBeNull();
        caixasPostais.Data.Count.ShouldBeGreaterThan(0);
        caixasPostais.Data.Count.ShouldBeLessThanOrEqualTo(10);
        caixasPostais.Data.ShouldNotBeNull();
        caixasPostais.TotalCount.ShouldBeGreaterThan(0);
        caixasPostais.TotalPages.ShouldBeGreaterThan(0);
        caixasPostais.PageNumber.ShouldBe(pageNumber);
        caixasPostais.PageSize.ShouldBe(pageSize);
    }

    [Fact(DisplayName = "Deve retornar a lista ordenada de caixas postais comunitárias")]
    [Trait("Integração", nameof(GetCaixasPostaisComunitariasEndpoint))]
    public async Task Deve_Retornar_Lista_Ordenada_De_Caixas_Postais_Comunitarias()
    {
        var orderBy = "id desc";

        var request = new GetCaixasPostaisComunitariasRequest()
        {
            OrderBy = orderBy,
            PageNumber = 1,
            PageSize = 200
        };

        var (response, caixasPostais) = await sut.Client.GETAsync<GetCaixasPostaisComunitariasEndpoint, GetCaixasPostaisComunitariasRequest, PagedResponse<GetCaixasPostaisComunitariasResponse>>(request);

        // Assert
        response.EnsureSuccessStatusCode();
        caixasPostais.ShouldNotBeNull();
        caixasPostais.Data.Count.ShouldBeGreaterThan(0);

        var caixasPostaisOrdenadas = caixasPostais.Data.OrderByDescending(c => c.Id).ToList();
        caixasPostais.Data.ShouldBe(caixasPostaisOrdenadas);
    }

    [Fact(DisplayName = "Deve retornar caixas postais comunitárias de acordo com o filtro")]
    [Trait("Integração", nameof(GetCaixasPostaisComunitariasEndpoint))]
    public async Task Deve_Retornar_Caixas_Postais_Comunitarias_De_Acordo_Com_O_Filtro()
    {
        var filter = "Nome=*Central";
        var request = new GetCaixasPostaisComunitariasRequest()
        {
            Filter = filter
        };

        var (response, caixasPostais) = await sut.Client.GETAsync<GetCaixasPostaisComunitariasEndpoint, GetCaixasPostaisComunitariasRequest, PagedResponse<GetCaixasPostaisComunitariasResponse>>(request);

        // Assert
        response.EnsureSuccessStatusCode();
        caixasPostais.ShouldNotBeNull();
        caixasPostais.Data.Count.ShouldBeGreaterThan(0);
        caixasPostais.Data.ShouldAllBe(c => c.Nome.Contains("Central", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Deve retornar 400 para filtro inválido")]
    [Trait("Integração", nameof(GetCaixasPostaisComunitariasEndpoint))]
    public async Task Deve_Retornar_400_Para_Filtro_Invalido()
    {
        var filter = "NaoExiste=Teste";
        var request = new GetCaixasPostaisComunitariasRequest()
        {
            Filter = filter
        };

        var (response, _) = await sut.Client.GETAsync<GetCaixasPostaisComunitariasEndpoint, GetCaixasPostaisComunitariasRequest, PagedResponse<GetCaixasPostaisComunitariasResponse>>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Deve retornar 400 para ordenação inválida")]
    [Trait("Integração", nameof(GetCaixasPostaisComunitariasEndpoint))]
    public async Task Deve_Retornar_400_Para_Ordenacao_Invalida()
    {
        var orderBy = "CampoInvalido desc";
        var request = new GetCaixasPostaisComunitariasRequest()
        {
            OrderBy = orderBy,
            PageNumber = 1,
            PageSize = 10
        };
        var (response, _) = await sut.Client.GETAsync<GetCaixasPostaisComunitariasEndpoint, GetCaixasPostaisComunitariasRequest, PagedResponse<GetCaixasPostaisComunitariasResponse>>(request);
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Deve retornar 400 para paginação inválida")]
    [Trait("Integração", nameof(GetCaixasPostaisComunitariasEndpoint))]
    public async Task Deve_Retornar_400_Para_Paginacao_Invalida()
    {
        var request = new GetCaixasPostaisComunitariasRequest()
        {
            PageNumber = 0,
            PageSize = 0
        };

        var (response, res) = await sut.Client.GETAsync<GetCaixasPostaisComunitariasEndpoint, GetCaixasPostaisComunitariasRequest, ErrorResponse>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        res.Errors.ShouldNotBeNull();
        res.Errors.Count.ShouldBe(2);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_NUMBER_QUERY);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_SIZE_QUERY);
    }
}

using Correios.DneBasico.Api.Features.Bairros;

namespace Correios.DneBasico.Api.IntegrationTests.Features.Bairros;

[Collection<SutCollection>]
public class GetBairrosEndpointTests(Sut sut) : TestBase
{
    [Fact(DisplayName = "Deve retornar os bairros paginados")]
    [Trait("Integração", nameof(GetBairrosEndpoint))]
    public async Task Deve_Retornar_Bairros_Paginados_HttpClient()
    {
        var pageNumber = 1;
        var pageSize = 10;

        var url = new StringBuilder();
        url.Append($"{ApiConstants.RouteNames.BAIRROS}?");
        url.Append($"{ApiConstants.RouteNames.PAGE_NUMBER_QUERY}={pageNumber}&");
        url.Append($"{ApiConstants.RouteNames.PAGE_SIZE_QUERY}={pageSize}");

        // Arrange & Act
        var request = new HttpRequestMessage(HttpMethod.Get, url.ToString());
        var response = await sut.Client.SendAsync(request, sut.Cancellation);

        // Assert
        response.EnsureSuccessStatusCode();

        var bairros = await response.Content.ReadFromJsonAsync<PagedResponse<GetBairrosResponse>>(cancellationToken: sut.Cancellation);
        bairros.ShouldNotBeNull();
        bairros.Data.Count.ShouldBeGreaterThan(0);
        bairros.Data.Count.ShouldBeLessThanOrEqualTo(10);
        bairros.Data.ShouldNotBeNull();
        bairros.TotalCount.ShouldBeGreaterThan(0);
        bairros.TotalPages.ShouldBeGreaterThan(0);
        bairros.PageNumber.ShouldBe(pageNumber);
        bairros.PageSize.ShouldBe(pageSize);
    }

    [Fact(DisplayName = "Deve retornar os bairros paginados (FastEndpoints)")]
    [Trait("Integração", nameof(GetBairrosEndpoint))]
    public async Task Deve_Retornar_Bairros_Paginados_FastEndpoints()
    {
        var pageNumber = 1;
        var pageSize = 10;

        var request = new GetBairrosRequest()
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var (response, bairros) = await sut.Client.GETAsync<GetBairrosEndpoint, GetBairrosRequest, PagedResponse<GetBairrosResponse>>(request);

        // Assert
        response.EnsureSuccessStatusCode();

        bairros.ShouldNotBeNull();
        bairros.Data.Count.ShouldBeGreaterThan(0);
        bairros.Data.Count.ShouldBeLessThanOrEqualTo(10);
        bairros.Data.ShouldNotBeNull();
        bairros.TotalCount.ShouldBeGreaterThan(0);
        bairros.TotalPages.ShouldBeGreaterThan(0);
        bairros.PageNumber.ShouldBe(pageNumber);
        bairros.PageSize.ShouldBe(pageSize);
    }

    [Fact(DisplayName = "Deve retornar os bairros de acordo com o filtro")]
    [Trait("Integração", nameof(GetBairrosEndpoint))]
    public async Task Deve_Retornar_Bairros_De_Acordo_Com_O_Filtro_HttpClient()
    {
        var filter = "Nome=*Centro";
        var url = new StringBuilder();
        url.Append($"{ApiConstants.RouteNames.BAIRROS}?");
        url.Append($"{ApiConstants.RouteNames.FILTER_QUERY}={Uri.EscapeDataString(filter)}");

        // Arrange & Act
        var request = new HttpRequestMessage(HttpMethod.Get, url.ToString());
        var response = await sut.Client.SendAsync(request, sut.Cancellation);

        // Assert
        response.EnsureSuccessStatusCode();
        var bairros = await response.Content.ReadFromJsonAsync<PagedResponse<GetBairrosResponse>>(cancellationToken: sut.Cancellation);
        bairros.ShouldNotBeNull();
        bairros.Data.Count.ShouldBeGreaterThan(0);
        bairros.Data.ShouldAllBe(b => b.Nome.Contains("Centro", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Deve retornar os bairros de acordo com o filtro (FastEndpoints)")]
    [Trait("Integração", nameof(GetBairrosEndpoint))]
    public async Task Deve_Retornar_Bairros_De_Acordo_Com_O_Filtro_FastEndpoints()
    {
        var filter = "Nome=*Centro";
        var request = new GetBairrosRequest()
        {
            Filter = filter
        };

        var (response, bairros) = await sut.Client.GETAsync<GetBairrosEndpoint, GetBairrosRequest, PagedResponse<GetBairrosResponse>>(request);

        // Assert
        response.EnsureSuccessStatusCode();
        bairros.ShouldNotBeNull();
        bairros.Data.Count.ShouldBeGreaterThan(0);
        bairros.Data.ShouldAllBe(b => b.Nome.Contains("Centro", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Deve retornar a lista de bairros ordenada por id desc (FastEndpoints)")]
    [Trait("Integração", nameof(GetBairrosEndpoint))]
    public async Task Deve_Retornar_Lista_De_Bairros_Ordenada_Por_Id_Desc_FastEndpoints()
    {
        var orderBy = "Id desc";

        var request = new GetBairrosRequest()
        {
            OrderBy = orderBy,
            PageNumber = 1,
            PageSize = 20
        };

        var (response, bairros) = await sut.Client.GETAsync<GetBairrosEndpoint, GetBairrosRequest, PagedResponse<GetBairrosResponse>>(request);

        // Assert     
        response.EnsureSuccessStatusCode();
        bairros.ShouldNotBeNull();
        bairros.Data.Count.ShouldBeGreaterThan(0);

        var bairrosOrdenados = bairros.Data.OrderByDescending(b => b.Id).ToList();
        bairros.Data.ShouldBe(bairrosOrdenados);
    }

    [Fact(DisplayName = "Deve retornar os bairros de acordo com o filtro de localidade (FastEndpoints)")]
    [Trait("Integração", nameof(GetBairrosEndpoint))]
    public async Task Deve_Retornar_Bairros_De_Acordo_Com_O_Filtro_De_Localidade_FastEndpoints()
    {
        var localidadeId = 9703; // Id da localidade Suzano/SP

        var filter = $"LocalidadeId={localidadeId}";

        var request = new GetBairrosRequest()
        {
            Filter = filter
        };

        var (response, bairros) = await sut.Client.GETAsync<GetBairrosEndpoint, GetBairrosRequest, PagedResponse<GetBairrosResponse>>(request);

        // Assert
        response.EnsureSuccessStatusCode();
        bairros.ShouldNotBeNull();
        bairros.Data.Count.ShouldBeGreaterThan(0);
        bairros.Data.ShouldAllBe(b => b.LocalidadeId == localidadeId);
    }

    [Fact(DisplayName = "Deve retornar erro 400 para filtro inválido (FastEndpoints)")]
    [Trait("Integração", nameof(GetBairrosEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Filtro_Invalido_FastEndpoints()
    {
        var filter = "PropriedadeInexistente=0";

        var request = new GetBairrosRequest()
        {
            Filter = filter
        };

        var (response, res) = await sut.Client.GETAsync<GetBairrosEndpoint, GetBairrosRequest, ErrorResponse>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }

    [Fact(DisplayName = "Deve retornar erro 400 para paginação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetBairrosEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Paginacao_Invalida_FastEndpoints()
    {
        var request = new GetBairrosRequest()
        {
            PageNumber = 0,
            PageSize = -5
        };

        var (response, res) = await sut.Client.GETAsync<GetBairrosEndpoint, GetBairrosRequest, ErrorResponse>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldNotBeNull();
        res.Errors.Count.ShouldBe(2);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_NUMBER_QUERY);
        res.Errors.ShouldContainKey(ApiConstants.RouteNames.PAGE_SIZE_QUERY);
    }

    [Fact(DisplayName = "Deve retornar erro 400 para ordenação inválida (FastEndpoints)")]
    [Trait("Integração", nameof(GetBairrosEndpoint))]
    public async Task Deve_Retornar_Erro_400_Para_Ordenacao_Invalida_FastEndpoints()
    {
        var request = new GetBairrosRequest()
        {
            OrderBy = "PropriedadeInexistente asc"
        };

        var (response, res) = await sut.Client.GETAsync<GetBairrosEndpoint, GetBairrosRequest, ErrorResponse>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        res.Errors.ShouldAllBe(e => e.Value.Contains(ApiConstants.GRIDYFY_INVALID_QUERY));
    }
}

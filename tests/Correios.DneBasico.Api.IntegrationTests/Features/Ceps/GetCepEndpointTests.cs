using Correios.DneBasico.Api.Features.Ceps;

namespace Correios.DneBasico.Api.IntegrationTests.Features.Ceps;

[Collection<SutCollection>]
public class GetCepEndpointTests(Sut sut) : TestBase
{
    [Fact(DisplayName = "Deve retornar um cep existente (FastEndpoints)")]
    [Trait("Integração", nameof(GetCepEndpoint))]
    public async Task Deve_Retornar_Um_Cep_Existente_FastEndpoints()
    {
        // Arrange
        var cepExistente = "01001000"; // assumindo que este CEP existe na base de testes
        var request = new GetCepRequest(cepExistente);

        // Act
        var (response, cep) = await sut.Client
            .GETAsync<GetCepEndpoint, GetCepRequest, GetCepResponse>(request);

        // Assert
        response.EnsureSuccessStatusCode();

        cep.ShouldNotBeNull();
        cep.Codigo.ShouldBe(cepExistente);
        cep.Municipio.ShouldNotBeNullOrWhiteSpace();
        cep.Uf.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact(DisplayName = "Deve retornar 404 quando o cep não existir")]
    [Trait("Integração", nameof(GetCepEndpoint))]
    public async Task Deve_Retornar_404_Quando_Cep_Nao_Existir()
    {
        // Arrange
        var cepInexistente = "99999999"; // CEP que não deve existir na base
        var request = new GetCepRequest(cepInexistente);

        // Act
        var (response, _) = await sut.Client
            .GETAsync<GetCepEndpoint, GetCepRequest, GetCepResponse>(request);

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Deve retornar 404 para cep inválido")]
    [Trait("Integração", nameof(GetCepEndpoint))]
    public async Task Deve_Retornar_404_Para_Cep_Invalido()
    {
        // Arrange
        var cepInvalido = "abc123"; // não bate com o regex ^\d{5}\-?\d{3}$
        var request = new GetCepRequest(cepInvalido);

        // Act
        var (response, _) = await sut.Client
            .GETAsync<GetCepEndpoint, GetCepRequest, GetCepResponse>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Theory(DisplayName = "Deve permitir cep com hífen ou não")]
    [Trait("Integração", nameof(GetCepEndpoint))]
    [InlineData("01001-000")]
    [InlineData("01001000")]
    public async Task Deve_Permitir_Cep_Com_Hifen_Ou_Nao(string cepInput)
    {
        // Arrange
        var request = new GetCepRequest(cepInput);

        // Act        
        var (response, cep) = await sut.Client
            .GETAsync<GetCepEndpoint, GetCepRequest, GetCepResponse>(request);

        // Assert
        response.EnsureSuccessStatusCode();

        cep.ShouldNotBeNull();
        cep.Codigo.ShouldBe("01001000"); // código normalizado sem hífen
    }
}

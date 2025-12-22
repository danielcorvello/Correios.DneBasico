using Correios.DneBasico.Domain.Enums;

namespace Correios.DneBasico.Api.Features.Ceps;

/// <summary>
/// Consulta um CEP específico.
/// </summary>
public class GetCepEndpoint : Endpoint<GetCepRequest, GetCepResponse>
{
    private readonly DneBasicoDbContext _dbContext;
    public GetCepEndpoint(DneBasicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get($"/{ApiConstants.RouteNames.CEPS}/{{Cep}}");
        AllowAnonymous();

        Description(b => b
            .WithTags(ApiConstants.Tags.BASE)
            .Accepts<GetCepRequest>()
            .Produces<GetCepResponse>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .ProducesProblemFE<ProblemDetails>((int)HttpStatusCode.BadRequest)
            .ProducesProblemFE<ProblemDetails>((int)HttpStatusCode.NotFound)
            .WithDisplayName(nameof(GetCepEndpoint)),
        clearDefaults: true);

        Summary(s =>
        {
            s.Summary = "Consulta um CEP específico.";
            s.Description = "Retorna os detalhes do CEP informado, se encontrado.";
            s.Response((int)HttpStatusCode.BadRequest, "Retorna um erro de validação se o CEP for inválido.");
            s.Response((int)HttpStatusCode.NotFound, "Retorna se o CEP não for encontrado na base de dados.");
            s.ResponseExamples[StatusCodes.Status200OK] = new GetCepResponse(
                Codigo: "01001000",
                Ibge: "3550308",
                Municipio: "São Paulo",
                Uf: "SP",
                Bairro: "Sé",
                Distrito: null,
                TipoLogradouro: "Praça",
                Logradouro: "da Sé",
                LogradouroCompleto: "Praça da Sé",
                Complemento: "- lado ímpar",
                Unidade: null,
                Geral: false,
                Tipo: TipoCep.LOG);
        });
    }

    public override async Task HandleAsync(GetCepRequest req, CancellationToken ct)
    {
        var cep = await _dbContext.Ceps
            .Where(c => c.Codigo == req.Cep)
            .FirstOrDefaultAsync(ct);

        if (cep is not null)
        {
            var response = new GetCepResponse(
                Codigo: cep.Codigo,
                Ibge: cep.Ibge,
                Municipio: cep.Municipio,
                Uf: cep.Uf,
                Bairro: cep.Bairro,
                Distrito: cep.Distrito,
                TipoLogradouro: cep.TipoLogradouro,
                Logradouro: cep.Logradouro,
                LogradouroCompleto: cep.LogradouroCompleto,
                Complemento: cep.Complemento,
                Unidade: cep.Unidade,
                Geral: cep.Geral,
                Tipo: cep.Tipo);

            await Send.OkAsync(response, cancellation: ct);
            return;
        }
        else
        {
            await Send.NotFoundAsync(cancellation: ct);
            return;
        }
    }
}

public record GetCepRequest(string Cep);

public class GetCepRequestValidator : Validator<GetCepRequest>
{
    public GetCepRequestValidator()
    {
        RuleFor(x => x.Cep)
            .NotEmpty().WithMessage("O CEP é obrigatório.")
            .Matches(@"^\d{8}$").WithMessage("O CEP deve conter exatamente 8 dígitos numéricos.");
    }
}
public record GetCepResponse(
    string Codigo,
    string Ibge,
    string Municipio,
    string Uf,
    string? Bairro,
    string? Distrito,
    string? TipoLogradouro,
    string? Logradouro,
    string? LogradouroCompleto,
    string? Complemento,
    string? Unidade,
    bool Geral,
    TipoCep Tipo);
namespace Correios.DneBasico.Api.Features._Shared;

public record QueryListRequest
{
    /// <summary>
    /// Filtro para busca.
    /// </summary>
    [QueryParam]
    public string? Filter { get; set; }

    /// <summary>
    /// Critério de ordenação dos resultados.
    /// </summary>
    [QueryParam]
    public string? OrderBy { get; set; }

    /// <summary>
    /// Página atual para paginação.
    /// </summary>
    [QueryParam]
    [DefaultValue(1)]
    public int? PageNumber { get; set; }

    /// <summary>
    /// Quantidade de itens por página para paginação.
    /// </summary>
    [QueryParam]
    [DefaultValue(100)]
    public int? PageSize { get; set; }
}

public class QueryListRequestValidator : Validator<QueryListRequest>
{
    public QueryListRequestValidator()
    {
        RuleFor(x => x.Filter!)
            .MinimumLength(3)
            .WithMessage("O filtro deve ter no mínimo 3 caracteres.");

        RuleFor(x => x.OrderBy!)
            .MinimumLength(2)
            .WithMessage("A ordenação deve ter no mínimo 2 caracteres.");

        When(x => x.PageNumber.HasValue || x.PageSize.HasValue, () =>
        {
            RuleFor(x => x.PageNumber)
                .NotNull()
                .WithMessage("O número da página é obrigatório quando o tamanho da página é fornecido.")
                .GreaterThan(0)
                .WithMessage("O número da página deve ser maior que zero.");

            RuleFor(x => x.PageSize)
                .NotNull()
                .WithMessage("O tamanho da página é obrigatório quando o número da página é fornecido.")
                .GreaterThan(0)
                .WithMessage("O tamanho da página deve ser maior que zero.");
        });
    }
}
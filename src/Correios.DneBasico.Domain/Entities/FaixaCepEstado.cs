namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Faixa de CEP do Estado
/// </summary>
public class FaixaCepEstado
{
    /// <summary>
    /// Sigla da Unidade Federativa (Estado)
    /// </summary>
    public string Uf { get; set; } = default!;

    /// <summary>
    /// CEP inicial do Estado
    /// </summary>
    public string CepInicial { get; set; } = default!;

    /// <summary>
    /// CEP final do Estado
    /// </summary>
    public string? CepFinal { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// Unidade Federativa (Estado)
    /// </summary>
    public Estado Estado { get; set; } = default!;
    #endregion
}
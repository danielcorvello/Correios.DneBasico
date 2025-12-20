namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Unidade Federativa (Estado) do Brasil
/// </summary>
public class Estado
{
    /// <summary>
    /// Sigla do Estado
    /// </summary>
    public string Uf { get; set; } = default!;

    /// <summary>
    /// Nome do Estado
    /// </summary>
    public string Nome { get; set; } = default!;

    /// <summary>
    /// Código IBGE do Estado
    /// </summary>    
    public string Ibge { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// Faixas de CEP do Estado
    /// </summary>
    public ICollection<FaixaCepEstado> Faixas { get; set; } = [];
    #endregion
}
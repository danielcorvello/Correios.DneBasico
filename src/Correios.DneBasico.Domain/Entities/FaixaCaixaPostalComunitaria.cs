using System.Text.Json.Serialization;

namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Faixa de Caixa Postal Comunitária
/// </summary>
public class FaixaCaixaPostalComunitaria
{
    /// <summary>
    /// Chave da caixa postal comunitária
    /// </summary>
    public int CaixaPostalComunitariaId { get; set; }

    /// <summary>
    /// Número inicial da caixa postal comunitária
    /// </summary>
    public string CaixaPostalInicial { get; set; } = default!;

    /// <summary>
    /// Número final da caixa postal comunitária
    /// </summary>
    public string CaixaPostalFinal { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// Caixa Postal Comunitária
    /// </summary>
    [JsonIgnore]
    public CaixaPostalComunitaria CaixaPostalComunitaria { get; set; } = default!;
    #endregion
}
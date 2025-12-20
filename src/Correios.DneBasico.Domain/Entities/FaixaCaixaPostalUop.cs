using System.Text.Json.Serialization;

namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Faixa de Caixa Postal – UOP
/// </summary>
public class FaixaCaixaPostalUop
{
    /// <summary>
    /// Chave da UOP
    /// </summary>
    public int UnidadeOperacionalId { get; set; }

    /// <summary>
    /// Número inicial da caixa postal
    /// </summary>
    public string CaixaPostalInicial { get; set; } = default!;

    /// <summary>
    /// Número final da caixa postal
    /// </summary>
    public string CaixaPostalFinal { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// Unidade Operacional
    /// </summary>
    [JsonIgnore]
    public UnidadeOperacional UnidadeOperacional { get; set; } = default!;
    #endregion
}
using Correios.DneBasico.Domain.Enums;

namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Faixa numérica do seccionamento
/// </summary>
public class FaixaNumericaSeccionamento
{
    /// <summary>
    /// Chave do logradouro
    /// </summary>
    public int LogradouroId { get; set; }

    /// <summary>
    /// Número inicial do seccionamento
    /// </summary>
    public string SeccionamentoInicial { get; set; } = default!;

    /// <summary>
    /// Número final do seccionamento
    /// </summary>
    public string SeccionamentoFinal { get; set; } = default!;

    /// <summary>
    /// Indica a paridade/lado do seccionamento
    /// </summary>
    /// <remarks>
    /// A – ambos,
    /// P – par,
    /// I – ímpar,
    /// D – direito e
    /// E – esquerdo.
    /// </remarks>
    public ParidadeLadoSeccionamento ParidadeLado { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// Logradouro
    /// </summary>
    public Logradouro Logradouro { get; set; } = default!;
    #endregion
}
using System.ComponentModel;

namespace Correios.DneBasico.Domain.Enums;

/// <summary>
/// Paridade / Lado do seccionamento
/// </summary>
public enum ParidadeLadoSeccionamento
{
    /// <summary>
    /// Ambos
    /// </summary>
    [Description("Ambos")]
    AMBOS = 1,

    /// <summary>
    /// Par
    /// </summary>
    [Description("Par")]
    PAR = 2,

    /// <summary>
    /// Ímpar
    /// </summary>
    [Description("Ímpar")]
    IMPAR = 3,

    /// <summary>
    /// Direito
    /// </summary>
    [Description("Direito")]
    DIREITO = 4,

    /// <summary>
    /// Esquerdo
    /// </summary>
    [Description("Esquerdo")]
    ESQUERDO = 5,
}
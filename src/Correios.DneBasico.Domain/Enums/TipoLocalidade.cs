using System.ComponentModel;

namespace Correios.DneBasico.Domain.Enums;

/// <summary>
/// Tipo da Localidade
/// </summary>
/// <remarks>
/// D – Distrito,
/// M – Município,
/// P – Povoado.
/// </remarks>
public enum TipoLocalidade
{
    /// <summary>
    /// Distrito
    /// </summary>
    [Description("Distrito")]
    DISTRITO = 1,

    /// <summary>
    /// Município
    /// </summary>
    [Description("Município")]
    MUNICIPIO = 2,

    /// <summary>
    /// Povoado
    /// </summary>
    [Description("Povoado")]
    POVOADO = 3,
}
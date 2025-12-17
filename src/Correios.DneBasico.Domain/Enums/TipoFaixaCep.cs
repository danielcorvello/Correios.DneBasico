using System.ComponentModel;

namespace Correios.DneBasico.Domain.Enums;

public enum TipoFaixaCep
{
    /// <summary>
    /// Total do Município
    /// </summary>
    [Description("Total do Município")]
    TOTAL_DO_MUNICIPIO = 1,

    /// <summary>
    /// Exclusiva da Sede Urbana
    /// </summary>
    [Description("Exclusiva da Sede Urbana")]
    EXCLUSIVA_SEDE_URBANA = 2,
}
using System.ComponentModel;

namespace Correios.DneBasico.Domain.Enums;

/// <summary>
/// Situação da Localidade
/// </summary>
/// <remarks>
/// 0 = Localidade  não codificada em nível de Logradouro,
/// 1 = Localidade codificada em nível de Logradouro
/// 2 = Distrito ou Povoado inserido na codificação em nível de Logradouro
/// 3 = Localidade em fase de codificação em nível de Logradouro
/// </remarks>
public enum SituacaoLocalidade
{
    /// <summary>
    /// Localidade não codificada em nível de Logradouro
    /// </summary>
    [Description("Não codificada")]
    NAO_CODIFICADA = 0,

    /// <summary>
    /// Localidade codificada em nível de Logradouro
    /// </summary>
    [Description("Localidade codificada")]
    CODIFICADA = 1,

    /// <summary>
    /// Distrito ou Povoado inserido na codificação em nível de Logradouro
    /// </summary>
    [Description("Distrito ou Povoado")]
    DISTRITO_OU_POVOADO = 2,

    /// <summary>
    /// Localidade em fase de codificação em nível de Logradouro
    /// </summary>
    [Description("Fase de Codificação")]
    FASE_DE_CODIFICACAO = 3,
}
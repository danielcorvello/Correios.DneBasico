using System.ComponentModel;

namespace Correios.DneBasico.Domain.Enums;

public enum TipoCep
{
    /// <summary>
    /// Localidade
    /// </summary>
    [Description("Localidade")]
    LOC = 1,

    /// <summary>
    /// Logradouro
    /// </summary>
    [Description("Logradouro")]
    LOG = 2,

    /// <summary>
    /// Grande Usuário
    /// </summary>
    [Description("Grande Usuário")]
    GU = 3,

    /// <summary>
    /// Unidade Operacional
    /// </summary>
    [Description("Unidade Operacional")]
    UOP = 4,

    /// <summary>
    /// Caixa Postal Comunitária
    /// </summary>
    [Description("Caixa Postal Comunitária")]
    CPC = 5
}
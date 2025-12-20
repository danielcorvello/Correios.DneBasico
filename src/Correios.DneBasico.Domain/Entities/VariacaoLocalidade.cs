using System.Text.Json.Serialization;

namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Outras denominações da Localidade (denominação popular, denominação anterior)
/// </summary>
public class VariacaoLocalidade
{
    /// <summary>
    /// Chave da localidade
    /// </summary>
    public int LocalidadeId { get; set; }

    /// <summary>
    /// Ordem da denominação
    /// </summary>
    public int Ordem { get; set; }

    /// <summary>
    /// Denominação
    /// </summary>
    public string Denominacao { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// Localidade
    /// </summary>
    [JsonIgnore]
    public Localidade Localidade { get; set; } = default!;
    #endregion
}
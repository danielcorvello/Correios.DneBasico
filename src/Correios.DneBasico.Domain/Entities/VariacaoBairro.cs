using System.Text.Json.Serialization;

namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Outras denominações do Bairro Localidade (denominação popular, denominação anterior)
/// </summary>
public class VariacaoBairro
{
    /// <summary>
    /// Cchave do bairro
    /// </summary>
    public int BairroId { get; set; }

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
    /// Bairro
    /// </summary>
    [JsonIgnore]
    public Bairro Bairro { get; set; } = default!;
    #endregion
}
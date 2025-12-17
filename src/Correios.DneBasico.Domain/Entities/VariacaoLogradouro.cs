using System.Text.Json.Serialization;

namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Outras denominações do logradouro
/// (denominação popular, denominação anterior)
/// </summary>
public class VariacaoLogradouro
{
    /// <summary>
    /// Chave do logradouro
    /// </summary>
    public int LogradouroId { get; set; }

    /// <summary>
    /// Ordem da denominação
    /// </summary>
    public int Ordem { get; set; }

    /// <summary>
    /// Tipo de logradouro da variação
    /// </summary>
    public string Tipo { get; set; } = default!;

    /// <summary>
    /// Nome da variação do logradouro
    /// </summary>
    public string Denominacao { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// Logradouro 
    /// </summary>
    [JsonIgnore]
    public Logradouro Logradouro { get; set; } = default!;
    #endregion
}
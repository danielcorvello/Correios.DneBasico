namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Faixa de CEP de Bairro
/// </summary>
public class FaixaCepBairro
{
    /// <summary>
    /// Chave do bairro
    /// </summary>
    public int BairroId { get; set; }

    /// <summary>
    /// CEP inicial do bairro
    /// </summary>
    public string CepInicial { get; set; } = default!;

    /// <summary>
    /// CEP final do bairro
    /// </summary>
    public string CepFinal { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// Bairro
    /// </summary>
    public Bairro Bairro { get; set; } = default!;
    #endregion
}
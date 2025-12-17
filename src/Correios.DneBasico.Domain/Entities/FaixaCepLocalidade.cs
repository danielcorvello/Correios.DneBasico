using Correios.DneBasico.Domain.Enums;

namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Faixa de CEP das Localidades codificadas
/// </summary>
/// <remarks>
/// Este arquivo contém dados relativos às faixas de CEP das localidades 
/// classificadas na categoria político-administrativa de município 
/// codificadas com CEP único ou codificadas por logradouros.
/// </remarks>
public class FaixaCepLocalidade
{
    /// <summary>
    /// chave da localidade
    /// </summary>
    public int LocalidadeId { get; set; }

    /// <summary>
    /// Localidade
    /// </summary>
    public Localidade Localidade { get; set; } = default!;

    /// <summary>
    /// CEP inicial da localidade
    /// </summary>
    public string CepInicial { get; set; } = default!;

    /// <summary>
    /// CEP final da localidade
    /// </summary>
    public string CepFinal { get; set; } = default!;

    /// <summary>
    /// tipo de Faixa de CEP: T –Total do Município, C – Exclusiva da  Sede Urbana
    /// </summary>
    public TipoFaixaCep TipoFaixa { get; set; } = default!;
}
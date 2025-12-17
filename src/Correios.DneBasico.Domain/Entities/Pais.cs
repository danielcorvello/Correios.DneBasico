namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Relação dos Nomes dos Países, suas siglas e grafias em inglês e francês
/// </summary>
public class Pais
{
    /// <summary>
    /// Sigla do País
    /// </summary>
    public string Sigla { get; set; } = default!;

    /// <summary>
    /// Sigla Alternativa do País
    /// </summary>
    public string SiglaAlternativa { get; set; } = default!;

    /// <summary>
    /// Nome do País em Português
    /// </summary>
    public string NomePortugues { get; set; } = default!;

    /// <summary>
    /// Nome do País em Inglês
    /// </summary>
    public string NomeIngles { get; set; } = default!;

    /// <summary>
    /// Nome do País em Francês
    /// </summary>
    public string NomeFrances { get; set; } = default!;

    /// <summary>
    /// Abreviatura do País
    /// </summary>
    public string? Abreviatura { get; set; }
}
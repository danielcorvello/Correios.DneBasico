using Correios.DneBasico.Domain.Enums;

namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// CEP - Tabela unificada
/// </summary>
public class Cep
{
    /// <summary>
    /// CEP - Código de Endereçamento Postal
    /// </summary>
    public string Codigo { get; set; } = default!;

    /// <summary>
    /// Código do município IBGE
    /// </summary>
    public string Ibge { get; set; } = default!;

    /// <summary>
    /// Município
    /// </summary>
    public string Municipio { get; set; } = default!;

    /// <summary>
    /// Sigla da UF
    /// </summary>
    public string Uf { get; set; } = default!;

    /// <summary>
    /// Bairro
    /// </summary>
    public string? Bairro { get; set; } = default!;

    /// <summary>
    /// Distrito
    /// </summary>
    public string? Distrito { get; set; }

    /// <summary>
    /// Tipo de logradouro
    /// </summary>
    public string? TipoLogradouro { get; set; } = default!;

    /// <summary>
    /// Logradouro
    /// </summary>
    public string? Logradouro { get; set; } = default!;

    /// <summary>
    /// Logradouro completo
    /// </summary>
    public string? LogradouroCompleto { get; set; } = default!;

    /// <summary>
    /// Complemento do logradouro
    /// </summary>
    public string? Complemento { get; set; }

    /// <summary>
    /// Unidade (Grande Usuário ou Unidade Operacional)
    /// </summary>
    public string? Unidade { get; set; } = default!;

    /// <summary>
    /// Indicador de CEP Geral
    /// </summary>
    public bool Geral { get; set; } = default!;

    /// <summary>
    /// Tipo de CEP
    /// </summary>
    public TipoCep Tipo { get; set; } = default!;

    /// <summary>
    /// Latitude
    /// </summary>
    public double? Lat { get; set; } = default!;

    /// <summary>
    /// Longitude
    /// </summary>
    public double? Lng { get; set; } = default!;
}
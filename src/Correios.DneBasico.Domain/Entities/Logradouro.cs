namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Logradouro
/// </summary>
/// <remarks>
/// Este arquivo contém os registros das localidades codificadas por 
/// logradouro(LOC_IN_SIT=1) e de localidades em fase de 
/// codificação(LOC_IN_SIT=3). Para encontrar o bairro do logradouro, 
/// utilize o campo BAI_NU_INI(relacionamento com LOG_BAIRRO, campo BAI_NU)..
/// </remarks>
public class Logradouro
{
    /// <summary>
    /// Chave do logradouro
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Sigla da UF
    /// </summary>
    public string Uf { get; set; } = default!;

    /// <summary>
    /// Chave da localidade
    /// </summary>
    public int LocalidadeId { get; set; }

    /// <summary>
    /// Chave do bairro inicial do logradouro
    /// </summary>
    public int BairroId { get; set; }

    /// <summary>
    /// Nome do logradouro
    /// </summary>
    public string Nome { get; set; } = default!;

    /// <summary>
    /// Complemento do logradouro (opcional)
    /// </summary>
    public string? Complemento { get; set; }

    /// <summary>
    /// CEP do logradouro
    /// </summary>
    public string Cep { get; set; } = default!;

    /// <summary>
    /// Tipo de logradouro
    /// </summary>
    public string Tipo { get; set; } = default!;

    /// <summary>
    /// Indicador de utilização do tipo de logradouro (S ou N) (opcional)
    /// </summary>
    public string? StatusTipo { get; set; }

    /// <summary>
    /// Abreviatura do nome do logradouro (opcional)
    /// </summary>
    public string? NomeAbreviado { get; set; }

    #region Navigation Properties
    /// <summary>
    /// Localidade
    /// </summary>
    public Localidade Localidade { get; set; } = default!;

    /// <summary>
    /// Bairro do logradouro
    /// </summary>
    public Bairro Bairro { get; set; } = default!;

    /// <summary>
    /// Variações do Logradouro
    /// </summary>
    public ICollection<VariacaoLogradouro> Variacoes { get; set; } = [];

    /// <summary>
    /// Faixas Numéricas de Seccionamento
    /// </summary>
    public ICollection<FaixaNumericaSeccionamento> FaixasNumericasSeccionamento { get; set; } = [];
    #endregion
}
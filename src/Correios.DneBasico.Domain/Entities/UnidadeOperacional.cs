namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Unidade Operacional dos Correios
/// </summary>
/// <remarks>
/// São agências próprias ou terceirizadas, centros de distribuição, etc.
/// O campo LOG_NU está sem conteúdo para as localidades não 
/// codificadas(LOC_IN_SIT=0), devendo ser utilizado o campo 
/// UOP_ENDERECO para endereçamento.
/// </remarks>
public class UnidadeOperacional
{
    /// <summary>
    /// chave da UOP
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// sigla da UF
    /// </summary>
    public string Uf { get; set; } = default!;

    /// <summary>
    /// chave da localidade
    /// </summary>
    public int LocalidadeId { get; set; }

    /// <summary>
    /// chave do bairro
    /// </summary>
    public int BairroId { get; set; }

    /// <summary>
    /// chave do logradouro (opcional)
    /// </summary>
    public int? LogradouroId { get; set; }

    /// <summary>
    /// nome da UOP
    /// </summary>
    public string Nome { get; set; } = default!;

    /// <summary>
    /// endereço da UOP
    /// </summary>
    public string Endereco { get; set; } = default!;

    /// <summary>
    /// CEP da UOP
    /// </summary>
    public string Cep { get; set; } = default!;

    /// <summary>
    /// indicador de caixa postal (S ou N)
    /// </summary>
    public string CaixasPostais { get; set; } = default!;

    /// <summary>
    /// abreviatura do nome da unid. operacional (opcional)
    /// </summary>
    public string? NomeAbreviado { get; set; }

    #region Navigation Properties
    /// <summary>
    /// Estado
    /// </summary>
    public Estado Estado { get; set; } = default!;
    /// <summary>
    /// Localidade
    /// </summary>
    public Localidade Localidade { get; set; } = default!;

    /// <summary>
    /// Bairro
    /// </summary>
    public Bairro Bairro { get; set; } = default!;

    /// <summary>
    /// Logradouro
    /// </summary>
    public Logradouro? Logradouro { get; set; }

    /// <summary>
    /// Faixas de Caixa Postal - UOP
    /// </summary>
    public ICollection<FaixaCaixaPostalUop> FaixasCaixaPostal { get; set; } = [];
    #endregion
}
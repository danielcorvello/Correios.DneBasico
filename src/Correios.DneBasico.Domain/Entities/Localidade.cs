using Correios.DneBasico.Domain.Enums;

namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// O arquivo LOG_LOCALIDADE contempla os municípios, distritos e povoados do Brasil.
/// </summary>
/// <remarks>
/// Os CEPs presentes neste arquivo valem para todos os logradouros da cidade, não necessitando consulta nos demais arquivos. 
/// 
/// As localidades em fase de codificação(LOC_IN_SIT= 3) estão em período de transição, sendo aceito o CEP Geral ou os CEPs de Logradouros para endereçamento.
/// </remarks>
public class Localidade
{
    /// <summary>
    /// Chave da localidade
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Sigla da UF
    /// </summary>
    public string Uf { get; set; } = default!;

    /// <summary>
    /// Nome da localidade
    /// </summary>
    public string Nome { get; set; } = default!;

    /// <summary>
    /// CEP da localidade (para  localidade  não codificada, ou seja loc_in_sit = 0) (opcional)
    /// </summary>
    public string? Cep { get; set; }

    /// <summary>
    /// Situação da localidade:
    /// </summary>
    /// <remarks>
    /// 0 = Localidade não codificada em nível de Logradouro,
    /// 1 = Localidade codificada em nível de Logradouro e
    /// 2 = Distrito ou Povoado inserido na codificação em nível de Logradouro.
    /// 3 = Localidade em fase de codificação em nível de Logradouro.
    /// </remarks>
    public SituacaoLocalidade Situacao { get; set; } = default!;

    /// <summary>
    /// Tipo de localidade
    /// </summary>  
    /// <remarks>
    /// D - Distrito,
    /// M - Município,
    /// P - Povoado.
    /// </remarks>
    public TipoLocalidade Tipo { get; set; } = default!;

    /// <summary>
    /// Chave da localidade de subordinação (opcional)
    /// </summary>
    public int? SubordinacaoId { get; set; }

    /// <summary>
    /// Abreviatura do nome da localidade (opcional)
    /// </summary>
    public string? NomeAbreviado { get; set; }

    /// <summary>
    /// Código do município IBGE (opcional)
    /// </summary>
    public string? Ibge { get; set; }

    #region Navigation Properties
    /// <summary>
    /// Estado (UF)
    /// </summary>
    public Estado Estado { get; set; } = default!;

    /// <summary>
    /// Localidade de subordinação
    /// </summary>
    public Localidade? Subordinacao { get; set; } = default!;

    /// <summary>
    /// Variações da Localidade
    /// </summary>
    public ICollection<VariacaoLocalidade> Variacoes { get; set; } = [];

    /// <summary>
    /// Faixas de CEP da Localidade
    /// </summary>
    public ICollection<FaixaCepLocalidade> Faixas { get; set; } = [];
    #endregion
}
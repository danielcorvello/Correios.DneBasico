namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Caixa Postal Comunitária(CPC) - são áreas rurais e/ou urbanas
/// periféricas não atendidas pela distribuição domiciliária.
/// </summary>
public class CaixaPostalComunitaria
{
    /// <summary>
    /// Chave da caixa postal comunitária
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
    /// Nome da CPC
    /// </summary>
    public string Nome { get; set; } = default!;

    /// <summary>
    /// Endereço da CPC
    /// </summary>
    public string Endereco { get; set; } = default!;

    /// <summary>
    /// CEP da CPC
    /// </summary>
    public string Cep { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// Localidade
    /// </summary>
    public Localidade Localidade { get; set; } = default!;

    /// <summary>
    /// Faixas de Caixa Postal Comunitária
    /// </summary>
    public ICollection<FaixaCaixaPostalComunitaria> Faixas { get; set; } = [];
    #endregion
}
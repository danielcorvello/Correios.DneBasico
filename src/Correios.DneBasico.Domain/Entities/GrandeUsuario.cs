namespace Correios.DneBasico.Domain.Entities;

/// <summary>
/// Grande Usuário
/// </summary>
/// <remarks>
/// São clientes com grande volume postal (empresas, universidades, bancos,
/// órgãos públicos, etc), O campo LOG_NU está sem conteúdo para as 
/// localidades não codificadas(LOC_IN_SIT=0), devendo ser utilizado o 
/// campo GRU_ENDERECO para  endereçamento.
/// </remarks>
public class GrandeUsuario
{
    /// <summary>
    /// chave do grande usuário
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
    /// nome do grande usuário
    /// </summary>
    public string Nome { get; set; } = default!;

    /// <summary>
    /// endereço do grande usuário
    /// </summary>
    public string Endereco { get; set; } = default!;

    /// <summary>
    /// CEP do grande usuário
    /// </summary>
    public string Cep { get; set; } = default!;

    /// <summary>
    /// abreviatura do nome do grande usuário (opcional)
    /// </summary>
    public string? NomeAbreviado { get; set; }

    #region Navigation Properties
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
    #endregion
}
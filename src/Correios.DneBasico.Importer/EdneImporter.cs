using Correios.DneBasico.Data.Contexts;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Correios.DneBasico.Importer;

public class EdneImporter
{
    private readonly int BATCH_SIZE = 5000;
    private readonly string BASEDIR = Path.Combine(AppContext.BaseDirectory, "Arquivos");
    private readonly CsvConfiguration csvConfig = null!;
    private readonly IServiceProvider _serviceProvider;

    public EdneImporter(IServiceProvider serviceProvider)
    {
        csvConfig = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            Delimiter = "@",
            Encoding = Encoding.Latin1,
            BadDataFound = null,
            ShouldQuote = args => true
        };

        _serviceProvider = serviceProvider;
    }

    public void ImportarArquivoCsv<TEntity, TMap>(string nomeArquivo)
        where TEntity : class
        where TMap : ClassMap<TEntity>
    {
        Console.WriteLine($"==========================================================");
        Console.WriteLine($"IMPORTANDO {nomeArquivo.Replace(".TXT", "")}");
        Console.WriteLine($"==========================================================");

        var files = new DirectoryInfo(string.Format(@"{0}", BASEDIR)).GetFiles(nomeArquivo);
        if (files.Length > 0)
        {
            foreach (var arquivo in files)
            {
                var watch = Stopwatch.StartNew();

                int counter = 0;

                Console.WriteLine();
                Console.WriteLine($"==========================================================");
                Console.WriteLine($"ABRINDO O ARQUIVO {arquivo.Name}");
                Console.WriteLine($"==========================================================");
                Console.WriteLine();

                using (var reader = new StreamReader(arquivo.FullName, encoding: Encoding.Latin1))
                using (var csv = new CsvReader(reader, csvConfig))
                {
                    csv.Context.RegisterClassMap<TMap>();
                    var records = csv.GetRecords<TEntity>();

                    if (typeof(TEntity) == typeof(Localidade))
                    {
                        // Ordena os registros de Localidade por SubordinadaId = null primeiro, para evitar problemas de FK e depois por Id
                        records = records.OrderBy(r => ((Localidade)(object)r).SubordinacaoId.HasValue)
                                                 .ThenBy(r => ((Localidade)(object)r).SubordinacaoId)
                                                 .ThenBy(r => ((Localidade)(object)r).Id);
                    }

                    if (typeof(TEntity) == typeof(FaixaCaixaPostalUop))
                    {
                        using var scope = _serviceProvider.CreateScope();
                        using var context = scope.ServiceProvider.GetRequiredService<DneBasicoDbContext>();

                        var uops = context.UnidadesOperacionais.AsNoTracking().Select(u => u.Id).ToHashSet();

                        records = records.Where(r => uops.Contains(((FaixaCaixaPostalUop)(object)r).UnidadeOperacionalId));
                    }

                    foreach (var batch in records.Chunk(BATCH_SIZE))
                    {
                        using var scope = _serviceProvider.CreateScope();
                        using var context = scope.ServiceProvider.GetRequiredService<DneBasicoDbContext>();

                        context.BulkInsert(batch.ToList());
                        counter += batch.Length;
                        Console.WriteLine("{0}", counter);
                    }
                }

                watch.Stop();
                TimeSpan t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);
                string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                        t.Hours,
                                        t.Minutes,
                                        t.Seconds,
                                        t.Milliseconds);
                Console.WriteLine($"Tempo de execução: {answer}");

                Console.WriteLine($"Arquivo {arquivo.Name} processado com {counter} registros.");
            }

            Console.WriteLine("");
            Console.WriteLine("");
        }
        else
        {
            Console.WriteLine($"Arquivo {nomeArquivo} não encontrado.");
        }
    }

    public async Task PovoarTabelaUnificadaAsync()
    {
        Console.WriteLine($"==========================================================");
        Console.WriteLine($"POVOANDO TABELA UNIFICADA");
        Console.WriteLine($"==========================================================");

        string sql = """
                    INSERT INTO ceps (codigo, ibge, municipio, uf, bairro, distrito, tipo_logradouro, logradouro, logradouro_completo, complemento, unidade, geral, tipo)
                    SELECT 
                            codigo, ibge, municipio, uf, bairro, distrito, tipo_logradouro, logradouro, logradouro_completo, complemento, unidade, geral, tipo
                    FROM (

                        SELECT localidades.loc_cep as codigo, localidades.mun_nu as ibge, localidades.loc_no as municipio, localidades.ufe_sg as uf, NULL as bairro, 
                        NULL as distrito, NULL as tipo_logradouro, 
                        NULL as logradouro, NULL as logradouro_completo, NULL as complemento, TRUE as geral, NULL as unidade, 1 as tipo
                        FROM localidades
                        WHERE localidades.loc_cep IS NOT NULL AND loc_nu_sub IS NULL

                        UNION

                        SELECT localidades.loc_cep as codigo, localidadesSub.mun_nu as ibge, localidadesSub.loc_no as municipio, localidades.ufe_sg as uf, NULL as bairro, 
                        CASE WHEN localidades.loc_in_tipo_loc = 1 OR localidades.loc_in_tipo_loc = 3 THEN localidades.loc_no ELSE NULL END as distrito, NULL as tipo_logradouro, 
                            NULL as logradouro, NULL as logradouro_completo, NULL as complemento, TRUE as geral, NULL as unidade, 1 as tipo
                        FROM localidades, localidades localidadesSub
                        WHERE localidades.loc_cep IS NOT NULL AND localidades.loc_nu_sub IS NOT NULL
                        AND localidades.loc_nu_sub = localidadesSub.loc_nu

                    UNION

                        SELECT logradouros.cep as codigo, localidades.mun_nu as ibge, localidades.loc_no as municipio, logradouros.ufe_sg as uf, bairros.bai_no as bairro, NULL as distrito, logradouros.tlo_tx as tipo, 
                        logradouros.log_no as municipio, (logradouros.tlo_tx || ' ' || logradouros.log_no) as logradouro_completo,  logradouros.log_complemento as complemento, FALSE as geral, NULL as unidade, 2 as tipo
                        FROM logradouros, localidades, bairros
                        WHERE logradouros.loc_nu = localidades.loc_nu  and logradouros.bai_nu_ini = bairros.bai_nu and logradouros.log_sta_tlo ='S'

                        UNION

                        SELECT logradouros.cep as codigo, localidades.mun_nu as ibge, localidades.loc_no as municipio, logradouros.ufe_sg as uf, bairros.bai_no as bairro, NULL as distrito, logradouros.tlo_tx as tipo, 
                        logradouros.log_no as municipio, (logradouros.log_no) as logradouro_completo,  logradouros.log_complemento as complemento, FALSE as geral, NULL as unidade, 2 as tipo
                        FROM logradouros, localidades, bairros
                        WHERE logradouros.loc_nu = localidades.loc_nu and logradouros.bai_nu_ini = bairros.bai_nu and logradouros.log_sta_tlo ='N'

                    UNION
                        SELECT grandes_usuarios.cep as codigo, localidades.mun_nu as ibge, localidades.loc_no as municipio, localidades.ufe_sg as uf, bairros.bai_no as bairro, NULL as distrito, logradouros.tlo_tx as tipo, 
                        logradouros.log_no as logradouro, (grandes_usuarios.gru_endereco) as logradouro_completo, NULL as complemento, FALSE as geral, grandes_usuarios.gru_no as unidade, 3 as tipo
                        FROM grandes_usuarios grandes_usuarios
                        join localidades ON localidades.loc_nu = grandes_usuarios.loc_nu
                        join bairros ON bairros.bai_nu = grandes_usuarios.bai_nu
                        left join logradouros ON logradouros.log_nu = grandes_usuarios.log_nu
                        WHERE localidades.mun_nu IS NOT NULL

                        UNION

                        SELECT grandes_usuarios.cep as codigo, localidadesSub.mun_nu as ibge, localidadesSub.loc_no as municipio, localidadesSub.ufe_sg as uf, bairros.bai_no as bairro, 
                        CASE  WHEN localidades.loc_in_tipo_loc = 1 OR localidades.loc_in_tipo_loc = 3 THEN localidades.loc_no ELSE NULL END as distrito, logradouros.tlo_tx as tipo, logradouros.LOG_NO as logradouro, 
                            (grandes_usuarios.gru_endereco) as logradouro_completo, NULL as complemento, FALSE as geral, grandes_usuarios.gru_no as unidade, 3 as tipo
                        FROM grandes_usuarios grandes_usuarios
                        join localidades ON localidades.loc_nu = grandes_usuarios.loc_nu
                        join localidades localidadesSub ON localidadesSub.loc_nu = localidades.loc_nu_sub
                        join bairros ON bairros.bai_nu = grandes_usuarios.bai_nu
                        left join logradouros ON logradouros.log_nu = grandes_usuarios.log_nu
                        WHERE localidades.loc_nu_sub IS NOT NULL 

                    UNION

                        SELECT unidades_operacionais.cep as codigo, localidades.mun_nu as ibge, localidades.loc_no as municipio, localidades.ufe_sg as uf, bairro.bai_no as bairro, NULL as distrito, logradouro.tlo_tx as tipo, logradouro.log_no as logradouro, 
                            (unidades_operacionais.UOP_ENDERECO) as logradouro_completo, NULL as complemento, FALSE as geral, unidades_operacionais.uop_no as unidade, 4 as tipo
                        FROM unidades_operacionais 
                        join localidades ON localidades.loc_nu = unidades_operacionais.loc_nu
                        join bairros bairro ON bairro.bai_nu = unidades_operacionais.bai_nu
                        left join logradouros logradouro ON logradouro.log_nu = unidades_operacionais.log_nu
                        WHERE localidades.mun_nu IS NOT NULL

                        UNION

                        SELECT unidades_operacionais.cep as codigo, localidadesSub.mun_nu as ibge, localidadesSub.loc_no as municipio, localidadesSub.ufe_sg as uf, bairros.bai_no as bairro, 
                        CASE  WHEN localidades.loc_in_tipo_loc = 1 OR localidades.loc_in_tipo_loc = 3 THEN localidades.loc_no ELSE NULL END as distrito, logradouros.tlo_tx as tipo, logradouros.LOG_NO as logradouro, 
                            (unidades_operacionais.UOP_ENDERECO) as logradouro_completo, NULL as complemento, FALSE as geral, unidades_operacionais.uop_no as unidade, 4 as tipo
                        FROM unidades_operacionais
                        join localidades ON localidades.loc_nu = unidades_operacionais.loc_nu
                        join localidades localidadesSub ON localidadesSub.loc_nu = localidades.loc_nu_sub
                        join bairros ON bairros.bai_nu = unidades_operacionais.bai_nu
                        left join logradouros ON logradouros.log_nu = unidades_operacionais.log_nu
                        WHERE localidades.loc_nu_sub IS NOT NULL

                    UNION

                        SELECT cpc.cep as codigo, localidades.mun_nu as ibge, localidades.loc_no as municipio, localidades.ufe_sg as uf, NULL as bairro, NULL as distrito, NULL as tipo, NULL as logradouro, 
                            (cpc.cpc_endereco) as logradouro_completo, NULL as complemento, FALSE as geral, cpc.cpc_no as unidade, 5 as tipo
                        FROM caixas_postais_comunitarias cpc
                        join localidades ON localidades.loc_nu = cpc.loc_nu
                        WHERE localidades.mun_nu IS NOT NULL 

                        UNION

                        SELECT cpc.cep as codigo, localidadesSub.mun_nu as ibge, localidadesSub.loc_no as municipio, localidadesSub.ufe_sg as uf, NULL as bairro, 
                        CASE  WHEN localidades.loc_in_tipo_loc = 1 OR localidades.loc_in_tipo_loc = 3 THEN localidades.loc_no ELSE NULL END as distrito, NULL as tipo, NULL as logradouro, 
                            (cpc.cpc_endereco) as logradouro_completo, NULL as complemento, FALSE as geral, cpc.cpc_no as unidade, 5 as tipo
                        FROM caixas_postais_comunitarias cpc
                        join localidades localidades ON localidades.loc_nu = cpc.loc_nu
                        join localidades localidadesSub ON localidadesSub.loc_nu = localidades.loc_nu_sub
                        WHERE localidades.loc_nu_sub IS NOT NULL
                    ) as dne
                    ORDER BY codigo
                    """;

        var watch = Stopwatch.StartNew();

        using var scope = _serviceProvider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DneBasicoDbContext>();
        context.Database.SetCommandTimeout(0);
        context.Database.ExecuteSqlRaw(sql);

        Console.WriteLine("TABELA CEP POVOADA COM OS DADOS DO E-DNE.");

        watch.Stop();

        TimeSpan t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);
        string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                t.Hours,
                                t.Minutes,
                                t.Seconds,
                                t.Milliseconds);

        Console.WriteLine($"Tempo de execução: {answer}");
    }
}
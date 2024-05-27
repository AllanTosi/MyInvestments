using System;
using System.Linq;
using System.Threading.Tasks;
using MyInvestments.Ativos;
using MyInvestments.ClasseAtivos;
using MyInvestments.Operacoes;
using MyInvestments.Setores;
using MyInvestments.TipoTransacoes;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace MyInvestments;

public class MyInvestmentsDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly ITipoTransacaoRepository _tipoTransacaoRepository;
    private readonly TipoTransacaoManager _tipoTransacaoManager;
    private readonly IClasseAtivoRepository _classeAtivoRepository;
    private readonly ClasseAtivoManager _classeAtivoManager;
    private readonly ISetorRepository _setorRepository;
    private readonly SetorManager _setorManager;
    private readonly IAtivoRepository _ativoRepository;
    private readonly AtivoManager _ativoManager;
    private readonly IOperacaoRepository _operacaoRepository;
    private readonly OperacaoManager _operacaoManager;

    public MyInvestmentsDataSeederContributor(
        ITipoTransacaoRepository tipoTransacaoRepository,
        TipoTransacaoManager tipoTransacaoManager,
        IClasseAtivoRepository classeAtivoRepository,
        ClasseAtivoManager classeAtivoManager,
        ISetorRepository setorRepository,
        SetorManager setorManager,
        IAtivoRepository ativoRepository,
        AtivoManager ativoManager,
        IOperacaoRepository operacaoRepository,
        OperacaoManager operacaoManager
        )
    {
        _tipoTransacaoRepository = tipoTransacaoRepository;
        _tipoTransacaoManager = tipoTransacaoManager;
        _classeAtivoRepository = classeAtivoRepository;
        _classeAtivoManager = classeAtivoManager;
        _setorRepository = setorRepository;
        _setorManager = setorManager;
        _ativoRepository = ativoRepository;
        _ativoManager = ativoManager;
        _operacaoRepository = operacaoRepository;
        _operacaoManager = operacaoManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        // Seed TipoTransacao
        if (await _tipoTransacaoRepository.GetCountAsync() <= 0)
        {
            await _tipoTransacaoRepository.InsertAsync(await _tipoTransacaoManager.CreateAsync("Compra"));
            await _tipoTransacaoRepository.InsertAsync(await _tipoTransacaoManager.CreateAsync("Venda"));
        }

        // Seed ClasseAtivo
        if (await _classeAtivoRepository.GetCountAsync() <= 0)
        {
            await _classeAtivoRepository.InsertAsync(await _classeAtivoManager.CreateAsync("Ação"));
            await _classeAtivoRepository.InsertAsync(await _classeAtivoManager.CreateAsync("FII"));
        }

        // Seed Setor
        if (await _setorRepository.GetCountAsync() <= 0)
        {
            await _setorRepository.InsertAsync(await _setorManager.CreateAsync("Financeiro"));
            await _setorRepository.InsertAsync(await _setorManager.CreateAsync("Logistica"));
            await _setorRepository.InsertAsync(await _setorManager.CreateAsync("Elétrico"));
        }

        // Get existing Setores and ClassesAtivo
        var setores = await _setorRepository.GetListAsync();
        var classesAtivo = await _classeAtivoRepository.GetListAsync();

        // Find IDs
        var logistica = setores.FirstOrDefault(s => s.Descricao == "Logistica")?.Id ?? Guid.Empty;
        var financeiro = setores.FirstOrDefault(s => s.Descricao == "Financeiro")?.Id ?? Guid.Empty;
        var eletrico = setores.FirstOrDefault(s => s.Descricao == "Elétrico")?.Id ?? Guid.Empty;
        var acao = classesAtivo.FirstOrDefault(ca => ca.Nome == "Ação")?.Id ?? Guid.Empty;
        var fii = classesAtivo.FirstOrDefault(ca => ca.Nome == "FII")?.Id ?? Guid.Empty;

        if (logistica == Guid.Empty || financeiro == Guid.Empty || eletrico == Guid.Empty || acao == Guid.Empty || fii == Guid.Empty)
        {
            throw new Exception("One or more related entities were not found.");
        }

        // Seed Ativo
        if (await _ativoRepository.GetCountAsync() <= 0)
        {
            await _ativoRepository.InsertAsync(
                await _ativoManager.CreateAsync(
                    "HGLG11",
                    "CSHG LOGISTICA FII",
                    fii,
                    logistica,
                    "Melhor FII"
                )
            );

            await _ativoRepository.InsertAsync(
                await _ativoManager.CreateAsync(
                    "BBAS3",
                    "BCO BRASIL S.A.",
                    acao,
                    financeiro,
                    "Primeiro banco do Brasil"
                )
            );

            await _ativoRepository.InsertAsync(
                await _ativoManager.CreateAsync(
                    "EGIE3",
                    "ENGIE BRASIL ENERGIA S.A.",
                    acao,
                    eletrico,
                    "Uma das melhores do setor Elétrico"
                )
            );
        }

        var ativo = await _ativoRepository.GetListAsync();

        // Find IDs
        var hglg = ativo.FirstOrDefault(s => s.Ticker == "HGLG11")?.Id ?? Guid.Empty;
        var egie = ativo.FirstOrDefault(s => s.Ticker == "EGIE3")?.Id ?? Guid.Empty;

        if (hglg == Guid.Empty || egie == Guid.Empty)
        {
            throw new Exception("One or more related entities were not found.");
        }

        // Seed Operações
        if (await _operacaoRepository.GetCountAsync() <= 0)
        {
            await _operacaoRepository.InsertAsync(
                await _operacaoManager.CreateAsync(
                    hglg,
                    new DateTime(2023, 12, 25),
                    100,
                    42.0f,
                    10,
                    11,
                    5
                    )
            );

            await _operacaoRepository.InsertAsync(
               await _operacaoManager.CreateAsync(
                   egie,
                   new DateTime(2024, 4, 28),
                   200,
                   45.0f,
                   2,
                   3,
                   1
                   )
           );
        }

    }
}
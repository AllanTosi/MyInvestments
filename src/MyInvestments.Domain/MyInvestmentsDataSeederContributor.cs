using System;
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
        // ADDED SEED DATA FOR TipoTransacao
        if (await _tipoTransacaoRepository.GetCountAsync() <= 0)
        {
            var compra = await _tipoTransacaoRepository.InsertAsync(
                await _tipoTransacaoManager.CreateAsync(
                    "Compra")
            );

            var venda = await _tipoTransacaoRepository.InsertAsync(
                await _tipoTransacaoManager.CreateAsync(
                    "Venda")
            );
        }

        // ADDED SEED DATA FOR ClasseAtivo
        if (await _classeAtivoRepository.GetCountAsync() <= 0)
        {
            var acao = await _classeAtivoRepository.InsertAsync(
                await _classeAtivoManager.CreateAsync(
                    "Ação")
            );

            var fii = await _classeAtivoRepository.InsertAsync(
                await _classeAtivoManager.CreateAsync(
                    "FII")
            );

            // ADDED SEED DATA FOR Setor
            if (await _setorRepository.GetCountAsync() <= 0)
            {
                var financeiro = await _setorRepository.InsertAsync(
                    await _setorManager.CreateAsync(
                        "Financeiro")
                );

                var logistica = await _setorRepository.InsertAsync(
                    await _setorManager.CreateAsync(
                        "Logistica")
                );

                var eletrico = await _setorRepository.InsertAsync(
                    await _setorManager.CreateAsync(
                        "Elétrico")
                );
            }


            //      Como adicionar os relacionamentos??

            // ADDED SEED DATA FOR Ativo
            //if (await _ativoRepository.GetCountAsync() <= 0)
            //{
            //    await _ativoRepository.InsertAsync(
            //        await _ativoManager.CreateAsync(
            //            "HGLG11",
            //            "CSHG LOGISTICA FII",
            //            "Melhor FII"
            //            )
            //    );

            //    await _ativoRepository.InsertAsync(
            //        await _ativoManager.CreateAsync(
            //            "BBAS3",
            //            "BCO BRASIL S.A.",
            //            "Primeiro banco do Brasil"
            //            )
            //    );

            //    await _ativoRepository.InsertAsync(
            //        await _ativoManager.CreateAsync(
            //            "EGIE3",
            //            "ENGIE BRASIL ENERGIA S.A.",
            //            "Uma das melhores do setor Elétrico"
            //            )
            //    );
            //}

            // ADDED SEED DATA FOR Operações
            //if (await _operacaoRepository.GetCountAsync() <= 0)
            //{
            //    await _operacaoRepository.InsertAsync(
            //        await _operacaoManager.CreateAsync(
            //            new DateTime(2023, 12, 25),
            //            100,
            //            42.0f,
            //            10,
            //            11,
            //            5
            //            )
            //    );

            //    await _operacaoRepository.InsertAsync(
            //       await _operacaoManager.CreateAsync(
            //           new DateTime(2024, 4, 28),
            //           200,
            //           45.0f,
            //           2,
            //           3,
            //           1
            //           )
            //   );
            //}

        }
    }
}
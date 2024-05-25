using MyInvestments.Ativos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MyInvestments.Operacoes;

public interface IOperacaoRepository : IRepository<Operacao, Guid>
{
    Task<Operacao> FindByDataOperacaoAsync(DateTime dataOperacao);

    Task<List<Operacao>> GetListByDataAsync(DateTime dataOperacao);

    Task<List<Operacao>> GetListWithRelationshipAsync();

    Task<List<Operacao>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null
    );


}

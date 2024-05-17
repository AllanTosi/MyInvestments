using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MyInvestments.Ativos;

public interface IAtivoRepository : IRepository<Ativo, Guid>
{
    Task<Ativo> FindByTickerAsync(string ticker);

    Task<Ativo> FindByNomeAsync(string nome);

    Task<List<Ativo>> GetListByTickerAsync(string ticker, Guid? userid = null);

    Task<List<Ativo>> GetListWithRelationshipAsync();

    Task<List<Ativo>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null
    );
}

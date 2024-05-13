using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyInvestments.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyInvestments.Operacoes;

public class EfCoreOperacaoRepository
    : EfCoreRepository<MyInvestmentsDbContext, Operacao, Guid>,
        IOperacaoRepository
{
    public EfCoreOperacaoRepository(
        IDbContextProvider<MyInvestmentsDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<Operacao> FindByDataOperacaoAsync(DateTime dataOperacao)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(operacao => operacao.DataOperacao.Equals(dataOperacao));
    }


    //Pensar em que tipo de lista preciso retornar

    public async Task<List<Operacao>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                operacao => operacao.DataOperacao.Equals(filter)
                )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }
}

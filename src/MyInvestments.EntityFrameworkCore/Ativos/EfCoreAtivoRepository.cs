using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using MyInvestments.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using MyInvestments.Setores;

namespace MyInvestments.Ativos;

public class EfCoreAtivoRepository
    : EfCoreRepository<MyInvestmentsDbContext, Ativo, Guid>,
        IAtivoRepository
{
    public EfCoreAtivoRepository(
        IDbContextProvider<MyInvestmentsDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<Ativo> FindByTickerAsync(string ticker)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(ativo => ativo.Ticker == ticker);
    }

    public async Task<Ativo> FindByNomeAsync(string nome)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(ativo => ativo.Nome == nome);
    }
    public async Task<List<Ativo>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                ativo => ativo.Ticker.Contains(filter)
                )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<List<Ativo>> GetListByTickerAsync(string ticker)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .Where(
                ativo => ativo.Ticker.ToLower().Contains(ticker)
                )
            .ToListAsync();
    }
}

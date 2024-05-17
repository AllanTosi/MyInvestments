using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyInvestments.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyInvestments.Setores;

public class EfCoreSetorRepository
    : EfCoreRepository<MyInvestmentsDbContext, Setor, Guid>,
        ISetorRepository
{
    public EfCoreSetorRepository(
        IDbContextProvider<MyInvestmentsDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<Setor> FindByDescricaoAsync(string descricao)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(setor => setor.Descricao == descricao);
    }

    public async Task<List<Setor>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                setor => setor.Descricao.Contains(filter)
                )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<List<Setor>> GetListByDescricaoAsync(string descricao)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .Where(
                setor => setor.Descricao.ToLower().Contains(descricao)
                )
            .ToListAsync();
    }

    public async Task<List<Setor>> GetListAllSetorAsync()
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.ToListAsync();
    }
}

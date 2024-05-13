using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyInvestments.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyInvestments.ClasseAtivos;

public class EfCoreClasseAtivoRepository
    : EfCoreRepository<MyInvestmentsDbContext, ClasseAtivo, Guid>,
        IClasseAtivoRepository
{
    public EfCoreClasseAtivoRepository(
        IDbContextProvider<MyInvestmentsDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<ClasseAtivo> FindByNomeAsync(string nome)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(classeAtivo => classeAtivo.Nome == nome);
    }

    public async Task<List<ClasseAtivo>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                classeAtivo => classeAtivo.Nome.Contains(filter)
                )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }
}

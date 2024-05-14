using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyInvestments.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyInvestments.TipoTransacoes;

public class EfCoreTipoTransacaoRepository
    : EfCoreRepository<MyInvestmentsDbContext, TipoTransacao, Guid>,
        ITipoTransacaoRepository
{
    public EfCoreTipoTransacaoRepository(
        IDbContextProvider<MyInvestmentsDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<TipoTransacao> FindByDescricaoAsync(string descricao)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(tipotransacao => tipotransacao.Descricao == descricao);
    }

    public async Task<List<TipoTransacao>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                tipotransacao => tipotransacao.Descricao.Contains(filter)
                )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<List<TipoTransacao>> GetListByDescricaoAsync(string descricao)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .Where(
                tipoTransacao => tipoTransacao.Descricao.ToLower().Contains(descricao)
                )
            .ToListAsync();
    }
}

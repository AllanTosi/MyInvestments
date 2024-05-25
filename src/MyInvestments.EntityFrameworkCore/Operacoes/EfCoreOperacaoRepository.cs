using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyInvestments.Ativos;
using MyInvestments.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        string filter = null,
        Guid? userId = null)
    {
        var dbSet = await GetDbSetAsync();
        
        var query = dbSet
            .Include(a => a.Ativo)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(operacao => operacao.DataOperacao.Equals(filter));
        }

        //Se passou id de usuário significa que tem Role User, e deve filtrar os registros
        if (userId != null)
            query = query.Where(x => x.CreatorId == userId);

        return await query
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<List<Operacao>> GetListByDataAsync(DateTime dataOperacao)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            /*.Where(
                operacao => operacao.DataOperacao.Equals( dataOperacao)
                )*/
            .Include(a => a.Ativo)
            .Where(operacao => operacao.DataOperacao.Date.Equals(dataOperacao))
            .ToListAsync();
    }

    public async Task<List<Operacao>> GetListWithRelationshipAsync(Guid? userId = null)
    {
        var dbSet = await GetDbSetAsync();

        var query = dbSet
            .Include(a => a.Ativo)
            .AsQueryable();

        //Se passou id de usuário significa que tem Role User, e deve filtrar os registros
        if (userId != null)
            query = query.Where(x => x.CreatorId == userId);

        return await query.ToListAsync();

/*        return await dbSet
            .Include(a => a.Ativo)
            .ToListAsync();*/
    }
}

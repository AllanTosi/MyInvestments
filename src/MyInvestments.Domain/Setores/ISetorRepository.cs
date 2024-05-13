using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MyInvestments.Setores;

public interface ISetorRepository : IRepository<Setor, Guid>
{
    Task<Setor> FindByDescricaoAsync(string descricao);

    Task<List<Setor>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null
    );
}

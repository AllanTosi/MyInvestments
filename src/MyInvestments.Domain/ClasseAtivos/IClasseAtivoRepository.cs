using MyInvestments.Setores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MyInvestments.ClasseAtivos;

public interface IClasseAtivoRepository : IRepository<ClasseAtivo, Guid>
{
    Task<ClasseAtivo> FindByNomeAsync(string nome);

    Task<List<ClasseAtivo>> GetListByNameAsync(string name);

    Task<List<ClasseAtivo>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null
    );
}

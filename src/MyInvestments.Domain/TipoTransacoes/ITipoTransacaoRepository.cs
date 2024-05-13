using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MyInvestments.TipoTransacoes;

public interface ITipoTransacaoRepository : IRepository<TipoTransacao, Guid>
{
    Task<TipoTransacao> FindByDescricaoAsync(string descricao);

    Task<List<TipoTransacao>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null
    );
}

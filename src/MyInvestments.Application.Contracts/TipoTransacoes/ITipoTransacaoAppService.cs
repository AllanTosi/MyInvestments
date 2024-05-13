using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyInvestments.TipoTransacoes;

public interface ITipoTransacaoAppService : IApplicationService
{
    Task<TipoTransacaoDto> GetAsync(Guid id);

    Task<PagedResultDto<TipoTransacaoDto>> GetListAsync(GetTipoTransacaoListDto input);

    Task<TipoTransacaoDto> CreateAsync(CreateTipoTransacaoDto input);

    Task UpdateAsync(Guid id, UpdateTipoTransacaoDto input);

    Task DeleteAsync(Guid id);
}
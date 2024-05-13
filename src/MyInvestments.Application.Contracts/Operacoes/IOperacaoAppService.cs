using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyInvestments.Operacoes;

public interface IOperacaoAppService : IApplicationService
{
    Task<OperacaoDto> GetAsync(Guid id);

    Task<PagedResultDto<OperacaoDto>> GetListAsync(GetOperacaoListDto input);

    Task<OperacaoDto> CreateAsync(CreateOperacaoDto input);

    Task UpdateAsync(Guid id, UpdateOperacaoDto input);

    Task DeleteAsync(Guid id);
}

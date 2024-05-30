using MyInvestments.TipoTransacoes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyInvestments.Operacoes;

public interface IOperacaoAppService : IApplicationService
{
    Task<OperacaoDto> GetAsync(Guid id);

    Task<PagedResultDto<OperacaoDto>> GetListAsync(GetOperacaoListDto input);

    Task<List<OperacaoDto>> GetListByDataAsync(DateTime dataOperacao);

    Task<ListResultDto<AtivoLookupDto>> GetAtivoLookupAsync();
    Task<ListResultDto<TipoTransacaoLookupDto>> GetTipoTransacaoLookupAsync();

    Task<OperacaoDto> CreateAsync(CreateOperacaoDto input);

    Task UpdateAsync(Guid id, UpdateOperacaoDto input);

    Task DeleteAsync(Guid id);
}

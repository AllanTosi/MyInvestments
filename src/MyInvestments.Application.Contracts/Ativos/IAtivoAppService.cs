using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyInvestments.Ativos;

public interface IAtivoAppService : IApplicationService
{
    Task<AtivoDto> GetAsync(Guid id);

    Task<PagedResultDto<AtivoDto>> GetListAsync(GetAtivoListDto input);

    Task<AtivoDto> CreateAsync(CreateAtivoDto input);

    Task UpdateAsync(Guid id, UpdateAtivoDto input);

    Task DeleteAsync(Guid id);

                //Adiciona Relacionamento
                Task<ListResultDto<ClasseAtivoLookupDto>> GetClasseAtivoLookupAsync();
                Task<ListResultDto<SetorLookupDto>> GetSetorLookupAsync();

}

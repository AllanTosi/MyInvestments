using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyInvestments.Setores;

public interface ISetorAppService : IApplicationService
{
    Task<SetorDto> GetAsync(Guid id);

    Task<PagedResultDto<SetorDto>> GetListAsync(GetSetorListDto input);

    Task<SetorDto> CreateAsync(CreateSetorDto input);

    Task UpdateAsync(Guid id, UpdateSetorDto input);

    Task DeleteAsync(Guid id);
}
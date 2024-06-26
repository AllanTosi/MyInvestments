﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyInvestments.Ativos;

public interface IAtivoAppService : IApplicationService
{
    Task<AtivoDto> GetAsync(Guid id);

    Task<PagedResultDto<AtivoDto>> GetListAsync(GetAtivoListDto input);
    
    Task<List<AtivoDto>> GetListByTickerAsync(string ticker);

    Task<ListResultDto<SetorLookupDto>> GetSetorLookupAsync();

    Task<ListResultDto<ClasseAtivoLookupDto>> GetClasseAtivoLookupAsync();

    Task<AtivoDto> CreateAsync(CreateAtivoDto input);

    Task UpdateAsync(Guid id, UpdateAtivoDto input);

    Task DeleteAsync(Guid id);
}

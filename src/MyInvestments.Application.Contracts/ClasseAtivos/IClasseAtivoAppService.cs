﻿using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyInvestments.ClasseAtivos;

public interface IClasseAtivoAppService : IApplicationService
{
    Task<ClasseAtivoDto> GetAsync(Guid id);

    Task<PagedResultDto<ClasseAtivoDto>> GetListAsync(GetClasseAtivoListDto input);

    Task<ClasseAtivoDto> CreateAsync(CreateClasseAtivoDto input);

    Task UpdateAsync(Guid id, UpdateClasseAtivoDto input);

    Task DeleteAsync(Guid id);
}

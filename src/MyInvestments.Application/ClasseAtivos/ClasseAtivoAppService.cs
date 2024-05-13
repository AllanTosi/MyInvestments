using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyInvestments.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace MyInvestments.ClasseAtivos;

//[Authorize(MyInvestmentsPermissions.ClasseAtivos.Default)]
public class ClasseAtivoAppService : MyInvestmentsAppService, IClasseAtivoAppService
{
    private readonly IClasseAtivoRepository _classeAtivoRepository;
    private readonly ClasseAtivoManager _classeAtivoManager;

    public ClasseAtivoAppService(
        IClasseAtivoRepository classeAtivoRepository,
        ClasseAtivoManager classeAtivoManager)
    {
        _classeAtivoRepository = classeAtivoRepository;
        _classeAtivoManager = classeAtivoManager;
    }

    //...SERVICE METHODS WILL COME HERE...
    public async Task<ClasseAtivoDto> GetAsync(Guid id)
    {
        var classeAtivo = await _classeAtivoRepository.GetAsync(id);
        return ObjectMapper.Map<ClasseAtivo, ClasseAtivoDto>(classeAtivo);
    }

    public async Task<PagedResultDto<ClasseAtivoDto>> GetListAsync(GetClasseAtivoListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(ClasseAtivo.Nome);
        }

        var classeAtivos = await _classeAtivoRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
        );

        var totalCount = input.Filter == null
            ? await _classeAtivoRepository.CountAsync()
            : await _classeAtivoRepository.CountAsync(
                classeAtivo => classeAtivo.Nome.Contains(input.Filter));

        return new PagedResultDto<ClasseAtivoDto>(
            totalCount,
            ObjectMapper.Map<List<ClasseAtivo>, List<ClasseAtivoDto>>(classeAtivos)
        );
    }

    //[Authorize(MyInvestmentsPermissions.ClasseAtivos.Create)]
    public async Task<ClasseAtivoDto> CreateAsync(CreateClasseAtivoDto input)
    {
        var classeAtivo = await _classeAtivoManager.CreateAsync(
            input.Nome
        );

        await _classeAtivoRepository.InsertAsync(classeAtivo);

        return ObjectMapper.Map<ClasseAtivo, ClasseAtivoDto>(classeAtivo);
    }

    //[Authorize(MyInvestmentsPermissions.ClasseAtivos.Edit)]
    public async Task UpdateAsync(Guid id, UpdateClasseAtivoDto input)
    {
        var classeAtivo = await _classeAtivoRepository.GetAsync(id);

        if (classeAtivo.Nome != input.Nome)
        {
            await _classeAtivoManager.ChangeNomeAsync(classeAtivo, input.Nome);
        }

        await _classeAtivoRepository.UpdateAsync(classeAtivo);
    }

    //[Authorize(MyInvestmentsPermissions.ClasseAtivos.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _classeAtivoRepository.DeleteAsync(id);
    }

}

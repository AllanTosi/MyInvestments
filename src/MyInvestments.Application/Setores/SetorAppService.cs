using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyInvestments.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace MyInvestments.Setores;

[Authorize(MyInvestmentsPermissions.Setores.Default)]
public class SetorAppService : MyInvestmentsAppService, ISetorAppService
{
    private readonly ISetorRepository _setorRepository;
    private readonly SetorManager _setorManager;

    public SetorAppService(
        ISetorRepository setorRepository,
        SetorManager setorManager)
    {
        _setorRepository = setorRepository;
        _setorManager = setorManager;
    }

    //...SERVICE METHODS WILL COME HERE...
    public async Task<SetorDto> GetAsync(Guid id)
    {
        var setor = await _setorRepository.GetAsync(id);
        return ObjectMapper.Map<Setor, SetorDto>(setor);
    }

    public async Task<PagedResultDto<SetorDto>> GetListAsync(GetSetorListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Setor.Descricao);
        }

        var setores = await _setorRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
        );

        var totalCount = input.Filter == null
            ? await _setorRepository.CountAsync()
            : await _setorRepository.CountAsync(
                setor => setor.Descricao.Contains(input.Filter));

        return new PagedResultDto<SetorDto>(
            totalCount,
            ObjectMapper.Map<List<Setor>, List<SetorDto>>(setores)
        );
    }

    [Authorize(MyInvestmentsPermissions.Setores.Create)]
    public async Task<SetorDto> CreateAsync(CreateSetorDto input)
    {
        var setor = await _setorManager.CreateAsync(
            input.Descricao
        );

        await _setorRepository.InsertAsync(setor);

        return ObjectMapper.Map<Setor, SetorDto>(setor);
    }

    [Authorize(MyInvestmentsPermissions.Setores.Edit)]
    public async Task UpdateAsync(Guid id, UpdateSetorDto input)
    {
        var setor = await _setorRepository.GetAsync(id);

        if (setor.Descricao != input.Descricao)
        {
            await _setorManager.ChangeDescricaoAsync(setor, input.Descricao);
        }

        await _setorRepository.UpdateAsync(setor);
    }

    [Authorize(MyInvestmentsPermissions.Setores.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _setorRepository.DeleteAsync(id);
    }

    [Authorize(MyInvestmentsPermissions.Setores.Default)]
    public async Task<List<SetorDto>> GetListByDescricaoAsync(string descricao)
    {
        var setores = await _setorRepository.GetListByDescricaoAsync(descricao);
        return ObjectMapper.Map<List<Setor>, List < SetorDto >> (setores);
    }
}

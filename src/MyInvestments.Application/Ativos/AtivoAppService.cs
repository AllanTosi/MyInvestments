using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyInvestments.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using MyInvestments.ClasseAtivos;
using MyInvestments.Setores;
using Volo.Abp.ObjectMapping;
using AutoMapper.Internal.Mappers;

namespace MyInvestments.Ativos;

//[Authorize(MyInvestmentsPermissions.Ativos.Default)]
public class AtivoAppService : MyInvestmentsAppService, IAtivoAppService
{
    private readonly IAtivoRepository _ativoRepository;
    private readonly AtivoManager _ativoManager;

                //Adiciona Relacionamento
                private readonly IClasseAtivoRepository _classeAtivoRepository;
                private readonly ClasseAtivoManager _classeAtivoManager;
                private readonly ISetorRepository _setorRepository;
                private readonly SetorManager _setorManager;

    public AtivoAppService(
        IAtivoRepository ativoRepository,
        AtivoManager ativoManager,

                    //Adiciona Relacionamento
                    IClasseAtivoRepository classeAtivoRepository,
                    ClasseAtivoManager classeAtivoManager,
                    ISetorRepository setorRepository,
                    SetorManager setorManager
        )
    {
        _ativoRepository = ativoRepository;
        _ativoManager = ativoManager;

                    //Adiciona Relacionamento
                    _classeAtivoRepository = classeAtivoRepository; 
                    _classeAtivoManager = classeAtivoManager;
                    _setorRepository = setorRepository;
                    _setorManager = setorManager;
    }

    //...SERVICE METHODS WILL COME HERE...

    public async Task<AtivoDto> GetAsync(Guid id)
    {
        var ativo = await _ativoRepository.GetAsync(id);
        return ObjectMapper.Map<Ativo, AtivoDto>(ativo);
    }

    public async Task<PagedResultDto<AtivoDto>> GetListAsync(GetAtivoListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Ativo.Ticker);
        }

        var ativos = await _ativoRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
        );

        var totalCount = input.Filter == null
            ? await _ativoRepository.CountAsync()
            : await _ativoRepository.CountAsync(
                ativo => ativo.Ticker.Contains(input.Filter));

        return new PagedResultDto<AtivoDto>(
            totalCount,
            ObjectMapper.Map<List<Ativo>, List<AtivoDto>>(ativos)
        );
    }

    //[Authorize(MyInvestmentsPermissions.Ativos.Create)]
    public async Task<AtivoDto> CreateAsync(CreateAtivoDto input)
    {
        var ativo = await _ativoManager.CreateAsync(
            input.Ticker,
            input.Nome,
            input.Descricao
        );

        await _ativoRepository.InsertAsync(ativo);

        return ObjectMapper.Map<Ativo, AtivoDto>(ativo);
    }

    //[Authorize(MyInvestmentsPermissions.Ativos.Edit)]
    public async Task UpdateAsync(Guid id, UpdateAtivoDto input)
    {
        var ativo = await _ativoRepository.GetAsync(id);

        if (ativo.Ticker != input.Ticker)
        {
            await _ativoManager.ChangeTickerAsync(ativo, input.Ticker);
        }

        if (ativo.Nome != input.Nome)
        {
            await _ativoManager.ChangeNomeAsync(ativo, input.Nome);
        }

        ativo.Descricao = input.Descricao;

        await _ativoRepository.UpdateAsync(ativo);
    }

    //[Authorize(MyInvestmentsPermissions.Ativos.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _ativoRepository.DeleteAsync(id);
    }

    //[Authorize(MyInvestmentsPermissions.Ativos.Default)]
    public async Task<List<AtivoDto>> GetListByTickerAsync(string ticker)
    {
        var ativos = await _ativoRepository.GetListByTickerAsync(ticker);
        return ObjectMapper.Map<List<Ativo>, List<AtivoDto>>(ativos);
    }

                //Adiciona Relacionamento
                public async Task<ListResultDto<ClasseAtivoLookupDto>> GetClasseAtivoLookupAsync()
                {
                    var classeAtivos = await _classeAtivoRepository.GetListAsync();

                    return new ListResultDto<ClasseAtivoLookupDto>(
                        ObjectMapper.Map<List<ClasseAtivo>, List<ClasseAtivoLookupDto>>(classeAtivos)
                    );
                }
                //Adiciona Relacionamento
                public async Task<ListResultDto<SetorLookupDto>> GetSetorLookupAsync()
                {
                    var setores = await _setorRepository.GetListAsync();

                    return new ListResultDto<SetorLookupDto>(
                        ObjectMapper.Map<List<Setor>, List<SetorLookupDto>>(setores)
                    );
                }

}

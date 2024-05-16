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
using DocumentFormat.OpenXml.Presentation;

namespace MyInvestments.Ativos;

//[Authorize(MyInvestmentsPermissions.Ativos.Default)]
public class AtivoAppService : MyInvestmentsAppService, IAtivoAppService
{
    private readonly IAtivoRepository _ativoRepository;
    private readonly AtivoManager _ativoManager;

    private readonly ISetorRepository _setorRepository;
    private readonly IClasseAtivoRepository _classeAtivoRepository;

    public AtivoAppService(
        IAtivoRepository ativoRepository,
        AtivoManager ativoManager,
        ISetorRepository setorRepository,
        IClasseAtivoRepository classeAtivoRepository
        )
    {
        _ativoRepository = ativoRepository;
        _ativoManager = ativoManager;
        _setorRepository = setorRepository;
        _classeAtivoRepository = classeAtivoRepository;
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

        var l = new List<AtivoDto>();

        foreach (var ativo in ativos)
        {
            var ativoDto = ObjectMapper.Map<Ativo, AtivoDto>(ativo);
            ativoDto.Setor = ObjectMapper.Map<Setor, SetorDto>(ativo.Setor);
            ativoDto.ClasseAtivo = ObjectMapper.Map<ClasseAtivo, ClasseAtivoDto>(ativo.ClasseAtivo);
            l.Add( ativoDto );
        }

        return new PagedResultDto<AtivoDto>(
            totalCount,
            l

        );
    }

    //[Authorize(MyInvestmentsPermissions.Ativos.Create)]
    public async Task<AtivoDto> CreateAsync(CreateAtivoDto input)
    {
        var ativo = await _ativoManager.CreateAsync(
            input.Ticker,
            input.Nome,
            input.SetorId,
            input.ClasseAtivoId,
            input.Descricao
        );

        ativo.ClasseAtivo = await _classeAtivoRepository.GetAsync(input.ClasseAtivoId);

        ativo.Setor = await _setorRepository.GetAsync(input.SetorId);

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

        ativo.SetorId = input.SetorId;

        ativo.ClasseAtivoId = input.ClasseAtivoId;

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

    //[Authorize(MyInvestmentsPermissions.Ativos.Default)]
    public async Task<ListResultDto<SetorLookupDto>> GetSetorLookupAsync()
    {
        var setores = await _setorRepository.GetListAsync();

        return new ListResultDto<SetorLookupDto>(
            ObjectMapper.Map<List<Setor>, List<SetorLookupDto>>(setores)
        );
    }

    //[Authorize(MyInvestmentsPermissions.Ativos.Default)]
    public async Task<ListResultDto<ClasseAtivoLookupDto>> GetClasseAtivoLookupAsync()
    {
        var classeAtivos = await _classeAtivoRepository.GetListAsync();

        return new ListResultDto<ClasseAtivoLookupDto>(
            ObjectMapper.Map<List<ClasseAtivo>, List<ClasseAtivoLookupDto>>(classeAtivos)
        );
    }

    //[Authorize(MyInvestmentsPermissions.Ativos.Default)]
    public async Task<List<AtivoDto>> GetListAllAtivoAsync()
    {
        var ativos = await _ativoRepository.GetListAllAtivoAsync();
        return ObjectMapper.Map<List<Ativo>, List<AtivoDto>>(ativos);
    }
}

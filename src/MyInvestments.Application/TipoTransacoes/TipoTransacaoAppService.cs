using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyInvestments.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace MyInvestments.TipoTransacoes;

//[Authorize(MyInvestmentsPermissions.TipoTransacoes.Default)]
public class TipoTransacaoAppService : MyInvestmentsAppService, ITipoTransacaoAppService
{
    private readonly ITipoTransacaoRepository _tipoTransacaoRepository;
    private readonly TipoTransacaoManager _tipoTransacaoManager;

    public TipoTransacaoAppService(
        ITipoTransacaoRepository tipoTransacaoRepository,
        TipoTransacaoManager tipoTransacaoManager)
    {
        _tipoTransacaoRepository = tipoTransacaoRepository;
        _tipoTransacaoManager = tipoTransacaoManager;
    }

    //...SERVICE METHODS WILL COME HERE...
    public async Task<TipoTransacaoDto> GetAsync(Guid id)
    {
        var tipoTransacao = await _tipoTransacaoRepository.GetAsync(id);
        return ObjectMapper.Map<TipoTransacao, TipoTransacaoDto>(tipoTransacao);
    }

    public async Task<PagedResultDto<TipoTransacaoDto>> GetListAsync(GetTipoTransacaoListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(TipoTransacao.Descricao);
        }

        var tipoTransacoes = await _tipoTransacaoRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
        );

        var totalCount = input.Filter == null
            ? await _tipoTransacaoRepository.CountAsync()
            : await _tipoTransacaoRepository.CountAsync(
                tipoTransacao => tipoTransacao.Descricao.Contains(input.Filter));

        return new PagedResultDto<TipoTransacaoDto>(
            totalCount,
            ObjectMapper.Map<List<TipoTransacao>, List<TipoTransacaoDto>>(tipoTransacoes)
        );
    }

    //[Authorize(MyInvestmentsPermissions.TipoTransacoes.Create)]
    public async Task<TipoTransacaoDto> CreateAsync(CreateTipoTransacaoDto input)
    {
        var tipoTransacao = await _tipoTransacaoManager.CreateAsync(
            input.Descricao
        );

        await _tipoTransacaoRepository.InsertAsync(tipoTransacao);

        return ObjectMapper.Map<TipoTransacao, TipoTransacaoDto>(tipoTransacao);
    }

    //[Authorize(MyInvestmentsPermissions.TipoTransacoes.Edit)]
    public async Task UpdateAsync(Guid id, UpdateTipoTransacaoDto input)
    {
        var tipoTransacao = await _tipoTransacaoRepository.GetAsync(id);

        if (tipoTransacao.Descricao != input.Descricao)
        {
            await _tipoTransacaoManager.ChangeDescricaoAsync(tipoTransacao, input.Descricao);
        }

        await _tipoTransacaoRepository.UpdateAsync(tipoTransacao);
    }

    //[Authorize(MyInvestmentsPermissions.TipoTransacoes.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _tipoTransacaoRepository.DeleteAsync(id);
    }

}

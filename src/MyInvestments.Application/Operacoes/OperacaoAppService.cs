using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyInvestments.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace MyInvestments.Operacoes;

//[Authorize(MyInvestmentsPermissions.Operacoes.Default)]
public class OperacaoAppService : MyInvestmentsAppService, IOperacaoAppService
{
    private readonly IOperacaoRepository _operacaoRepository;
    private readonly OperacaoManager _operacaoManager;

    public OperacaoAppService(
        IOperacaoRepository operacaoRepository,
        OperacaoManager operacaoManager)
    {
        _operacaoRepository = operacaoRepository;
        _operacaoManager = operacaoManager;
    }

    //...SERVICE METHODS WILL COME HERE...
    public async Task<OperacaoDto> GetAsync(Guid id)
    {
        var operacao = await _operacaoRepository.GetAsync(id);
        return ObjectMapper.Map<Operacao, OperacaoDto>(operacao);
    }

    public async Task<PagedResultDto<OperacaoDto>> GetListAsync(GetOperacaoListDto input)
    {
        if (input.Sorting.IsNullOrEmpty())
        {
            input.Sorting = nameof(Operacao.DataOperacao);
        }

        var operacoes = await _operacaoRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
        );

        var totalCount = input.Filter == null
            ? await _operacaoRepository.CountAsync()
            : await _operacaoRepository.CountAsync(
                operacao => operacao.DataOperacao.Equals(input.Filter));

        return new PagedResultDto<OperacaoDto>(
            totalCount,
            ObjectMapper.Map<List<Operacao>, List<OperacaoDto>>(operacoes)
        );
    }

    //[Authorize(MyInvestmentsPermissions.Operacoes.Create)]
    public async Task<OperacaoDto> CreateAsync(CreateOperacaoDto input)
    {
        var operacao = await _operacaoManager.CreateAsync(
            input.DataOperacao,
            input.Quantidade,
            input.Preco,
            input.ValorEmulumento,
            input.ValorIrpf,
            input.ValorCorretagem
        );

        await _operacaoRepository.InsertAsync(operacao);

        return ObjectMapper.Map<Operacao, OperacaoDto>(operacao);
    }

    //[Authorize(MyInvestmentsPermissions.Operacoes.Edit)]
    public async Task UpdateAsync(Guid id, UpdateOperacaoDto input)
    {
        var operacao = await _operacaoRepository.GetAsync(id);

        operacao.DataOperacao = input.DataOperacao;
        operacao.Quantidade = input.Quantidade;
        operacao.Preco = input.Preco;
        operacao.ValorEmulumento = input.ValorEmulumento;
        operacao.ValorIrpf = input.ValorIrpf;
        operacao.ValorCorretagem = input.ValorCorretagem;

        await _operacaoRepository.UpdateAsync(operacao);
    }

    //[Authorize(MyInvestmentsPermissions.Operacoes.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _operacaoRepository.DeleteAsync(id);
    }

    public async Task<List<OperacaoDto>> GetListByDataAsync(DateTime dataOperacao)
    {
        var operacoes = await _operacaoRepository.GetListByDataAsync(dataOperacao);
        return ObjectMapper.Map<List<Operacao>, List<OperacaoDto>>(operacoes);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyInvestments.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using MyInvestments.Ativos;
using MyInvestments.Setores;

namespace MyInvestments.Operacoes;

[Authorize(MyInvestmentsPermissions.Operacoes.Default)]
public class OperacaoAppService : MyInvestmentsAppService, IOperacaoAppService
{
    private readonly IOperacaoRepository _operacaoRepository;
    private readonly OperacaoManager _operacaoManager;

    private readonly IAtivoRepository _ativoRepository;

    public OperacaoAppService(
        IOperacaoRepository operacaoRepository,
        OperacaoManager operacaoManager,
        IAtivoRepository ativoRepository
        )
    {
        _operacaoRepository = operacaoRepository;
        _operacaoManager = operacaoManager;
        _ativoRepository = ativoRepository;
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


        var operacoes = new List<Operacao>();
        if (CurrentUser.IsInRole("admin"))
        {
            operacoes = await _operacaoRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
            );
        }
        else
        {
            operacoes = await _operacaoRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter,
            CurrentUser.Id
            );
        }

        var totalCount = input.Filter == null
            ? await _operacaoRepository.CountAsync()
            : await _operacaoRepository.CountAsync(
                operacao => operacao.DataOperacao.Equals(input.Filter));

        var listOperacaoDto = new List<OperacaoDto>();

        foreach (var operacao in operacoes)
        {
            var operacaoDto = ObjectMapper.Map<Operacao, OperacaoDto>(operacao);
            operacaoDto.Ativo = ObjectMapper.Map<Ativo, AtivoDto>(operacao.Ativo);
            listOperacaoDto.Add(operacaoDto);
        }

        return new PagedResultDto<OperacaoDto>(
            totalCount,
            ObjectMapper.Map<List<Operacao>, List<OperacaoDto>>(operacoes)
        );
    }

    [Authorize(MyInvestmentsPermissions.Operacoes.Create)]
    public async Task<OperacaoDto> CreateAsync(CreateOperacaoDto input)
    {
        var operacao = await _operacaoManager.CreateAsync(
            input.AtivoId,
            input.DataOperacao,
            input.Quantidade,
            input.Preco,
            input.ValorEmulumento,
            input.ValorIrpf,
            input.ValorCorretagem
        );

        operacao.Ativo = await _ativoRepository.GetAsync(input.AtivoId);

        await _operacaoRepository.InsertAsync(operacao);

        return ObjectMapper.Map<Operacao, OperacaoDto>(operacao);
    }

    [Authorize(MyInvestmentsPermissions.Operacoes.Edit)]
    public async Task UpdateAsync(Guid id, UpdateOperacaoDto input)
    {
        var operacao = await _operacaoRepository.GetAsync(id);

        operacao.DataOperacao = input.DataOperacao;
        operacao.Quantidade = input.Quantidade;
        operacao.Preco = input.Preco;
        operacao.ValorEmulumento = input.ValorEmulumento;
        operacao.ValorIrpf = input.ValorIrpf;
        operacao.ValorCorretagem = input.ValorCorretagem;

        operacao.AtivoId = input.AtivoId;

        await _operacaoRepository.UpdateAsync(operacao);
    }

    [Authorize(MyInvestmentsPermissions.Operacoes.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _operacaoRepository.DeleteAsync(id);
    }

    public async Task<List<OperacaoDto>> GetListByDataAsync(DateTime dataOperacao)
    {
        var operacoes = new List<Operacao>();
        if (CurrentUser.IsInRole("admin"))
        {
            operacoes = await _operacaoRepository.GetListByDataAsync(
                dataOperacao
            );
        }
        else
        {
            operacoes = await _operacaoRepository.GetListByDataAsync(
                dataOperacao,
                CurrentUser.Id
            );
        }
        
        return ObjectMapper.Map<List<Operacao>, List<OperacaoDto>>(operacoes);
    }

    [Authorize(MyInvestmentsPermissions.Operacoes.Default)]
    public async Task<ListResultDto<AtivoLookupDto>> GetAtivoLookupAsync()
    {
        var ativos = await _ativoRepository.GetListAsync();

        return new ListResultDto<AtivoLookupDto>(
            ObjectMapper.Map<List<Ativo>, List<AtivoLookupDto>>(ativos)
        );
    }

    [Authorize(MyInvestmentsPermissions.Operacoes.Default)]
    public async Task<List<OperacaoDto>> GetListAllAtivoAsync()
    {
        var operacoes = await _operacaoRepository.GetListWithRelationshipAsync();
        return ObjectMapper.Map<List<Operacao>, List<OperacaoDto>>(operacoes);
    }
}

using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace MyInvestments.TipoTransacoes;

public class TipoTransacaoManager : DomainService
{
    private readonly ITipoTransacaoRepository _tipoTransacaoRepository;

    public TipoTransacaoManager(ITipoTransacaoRepository tipoTransacaoRepository)
    {
        _tipoTransacaoRepository = tipoTransacaoRepository;
    }

    public async Task<TipoTransacao> CreateAsync(
        string descricao)
    {
        Check.NotNullOrWhiteSpace(descricao, nameof(descricao));

        var existingTipoTransacao = await _tipoTransacaoRepository.FindByDescricaoAsync(descricao);
        if (existingTipoTransacao != null)
        {
            throw new TipoTransacaoAlreadyExistsException(descricao);
        }

        return new TipoTransacao(
            GuidGenerator.Create(),
            descricao
        );
    }

    public async Task ChangeDescricaoAsync(
        TipoTransacao tipoTransacao,
        string novaDescricao)
    {
        Check.NotNull(tipoTransacao, nameof(tipoTransacao));
        Check.NotNullOrWhiteSpace(novaDescricao, nameof(novaDescricao));

        var existingTipoTransacao = await _tipoTransacaoRepository.FindByDescricaoAsync(novaDescricao);
        if (existingTipoTransacao != null && existingTipoTransacao.Id != tipoTransacao.Id)
        {
            throw new TipoTransacaoAlreadyExistsException(novaDescricao);
        }

        tipoTransacao.ChangeDescricao(novaDescricao);
    }
}

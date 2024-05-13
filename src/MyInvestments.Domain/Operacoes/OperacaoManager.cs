﻿using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace MyInvestments.Operacoes;

public class OperacaoManager : DomainService
{
    private readonly IOperacaoRepository _operacaoRepository;

    public OperacaoManager(IOperacaoRepository operacaoRepository)
    {
        _operacaoRepository = operacaoRepository;
    }

    public async Task<Operacao> CreateAsync(
        DateTime dataOperacao,
        int quantidade,
        float preco,
        float valorEmulumento,
        float valorIrpf,
        float? valorCorretagem = 0)
    {
        Check.NotNull(dataOperacao, nameof(dataOperacao));
        Check.NotNull(quantidade, nameof(quantidade));
        Check.Positive(quantidade, nameof(quantidade));
        Check.NotNull(preco, nameof(preco));
        Check.Positive(preco, nameof(preco));
        Check.NotNull(valorEmulumento, nameof(valorEmulumento));
        Check.Positive(valorEmulumento, nameof(valorEmulumento));
        Check.NotNull(valorIrpf, nameof(valorIrpf));
        Check.Positive(valorIrpf, nameof(valorIrpf));

        return new Operacao(
            GuidGenerator.Create(),
            dataOperacao,
            quantidade,
            preco,
            valorEmulumento,
            valorIrpf,
            valorCorretagem
        );
    }

    
}

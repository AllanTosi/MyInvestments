using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyInvestments.Operacoes;

public class Operacao : FullAuditedAggregateRoot<Guid>
{
    
    public DateTime DataOperacao { get; set; }
    public int Quantidade { get; set; }
    public float Preco { get; set; }
    public float ValorEmulumento { get; set; }
    public float ValorIrpf { get; set; }
    public float? ValorCorretagem { get; set; }

    private Operacao()
    {
        /* This constructor is for deserialization / ORM purpose */
    }

    internal Operacao(
        Guid id,
        DateTime dataOperacao,
        int quantidade,
        float preco,
        float valorEmulumento,
        float valorIrpf,
        float? valorCorretagem = null)
        : base(id)
    {
        DataOperacao = dataOperacao;
        Quantidade = quantidade;
        Preco = preco;
        ValorEmulumento = valorEmulumento;
        ValorIrpf = valorIrpf;
        ValorCorretagem = (float)valorCorretagem;
    }


}

using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyInvestments.TipoTransacoes;

public class TipoTransacao : FullAuditedAggregateRoot<Guid>
{
    public string Descricao { get; private set; }

    private TipoTransacao()
    {
        /* This constructor is for deserialization / ORM purpose */
    }

    internal TipoTransacao(
        Guid id,
        string descricao)
        : base(id)
    {
        SetDescricao(descricao);
    }

    internal TipoTransacao ChangeDescricao(string descricao)
    {
        SetDescricao(descricao);
        return this;
    }

    private void SetDescricao(string descricao)
    {
        Descricao = descricao;
        //Descricao = Check.NotNullOrWhiteSpace(
        //    descricao,
        //    nameof(descricao)
        //);
    }
}

using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyInvestments.Setores;

public class Setor : FullAuditedAggregateRoot<Guid>
{
    public string Descricao { get; private set; }

    private Setor()
    {
        /* This constructor is for deserialization / ORM purpose */
    }

    internal Setor(
        Guid id,
        string descricao)
        : base(id)
    {
        SetDescricao(descricao);
    }

    internal Setor ChangeDescricao(string descricao)
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

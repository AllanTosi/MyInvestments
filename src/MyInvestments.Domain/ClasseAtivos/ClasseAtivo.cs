using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyInvestments.ClasseAtivos;

public class ClasseAtivo : FullAuditedAggregateRoot<Guid>
{
    public string Nome { get; private set; }

    private ClasseAtivo()
    {
        /* This constructor is for deserialization / ORM purpose */
    }

    internal ClasseAtivo(
        Guid id,
        string nome)
        : base(id)
    {
        SetNome(nome);
    }

    internal ClasseAtivo ChangeNome(string nome)
    {
        SetNome(nome);
        return this;
    }

    private void SetNome(string nome)
    {
        Nome = Check.NotNullOrWhiteSpace(
            nome,
            nameof(nome),
            maxLength: ClasseAtivoConsts.MaxNomeLength
        );
    }
}

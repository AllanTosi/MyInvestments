using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyInvestments.Ativos;

public class Ativo : FullAuditedAggregateRoot<Guid>
{
    public string Ticker { get; private set; }
    public string Nome { get; private set; }    
    //public int Cnpj { get; set; }
    public string Descricao { get; set; }

                //Adicionando relacionameno
                public Guid ClasseAtivoId { get; set; }
                public Guid SetorId { get; set; }

    private Ativo()
    {
        /* This constructor is for deserialization / ORM purpose */
    }

    internal Ativo(Guid id,
        string ticker,
        string nome,
        string? descricao = null
        //,
        //Guid classeAtivoId,
        //Guid setorId
        )
        : base(id)
    {
        SetNome(nome);
        SetTicker(ticker);
        Descricao = descricao;
        //ClasseAtivoId = classeAtivoId;
        //SetorId = setorId;
    }

    internal Ativo ChangeNome(string nome)
    {
        SetNome(nome);
        return this;
    }

    private void SetNome(string nome)
    {
        Nome = Check.NotNullOrWhiteSpace(
            nome,
            nameof(nome),
            maxLength: AtivoConsts.MaxNomeLength
        );
    }
    internal Ativo ChangeTicker(string ticker)
    {
        SetTicker(ticker);
        return this;
    }
    private void SetTicker(string ticker)
    {
        Ticker = Check.NotNullOrWhiteSpace(
            ticker,
            nameof(ticker),
            maxLength: AtivoConsts.MaxTickerLength
        );
    }
}


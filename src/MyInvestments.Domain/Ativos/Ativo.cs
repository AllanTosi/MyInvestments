using System;
using JetBrains.Annotations;
using MyInvestments.ClasseAtivos;
using MyInvestments.Setores;
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
    public ClasseAtivo ClasseAtivo { get; set; }
    public Guid SetorId { get; set; }
    public Setor Setor    { get; set; }

    private Ativo()
    {
        /* This constructor is for deserialization / ORM purpose */
    }

    internal Ativo(Guid id,
        string ticker,
        string nome,
        Guid classeAtivoId,
        Guid setorId,
        string? descricao = null
        )
        : base(id)
    {
        SetNome(nome);
        SetTicker(ticker);
        ClasseAtivoId = classeAtivoId;
        SetorId = setorId;
        Descricao = descricao;
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


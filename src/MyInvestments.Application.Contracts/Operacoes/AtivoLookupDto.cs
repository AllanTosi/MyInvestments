using MyInvestments.ClasseAtivos;
using MyInvestments.Setores;
using System;
using Volo.Abp.Application.Dtos;

namespace MyInvestments.Operacoes;

public class AtivoLookupDto : EntityDto<Guid>
{
    public string Ticker { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
}

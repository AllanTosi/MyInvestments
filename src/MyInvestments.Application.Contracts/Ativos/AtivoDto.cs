using MyInvestments.ClasseAtivos;
using MyInvestments.Setores;
using System;
using Volo.Abp.Application.Dtos;

namespace MyInvestments.Ativos;

public class AtivoDto : EntityDto<Guid>
{
    public string Ticker { get; set; }
    public string Nome { get; set; }

    //public int Cnpj { get; set; }

    public string Descricao { get; set; }

    //Adicionando relacionamento
    public SetorDto Setor { get; set; }
    public ClasseAtivoDto ClasseAtivo { get; set; }
}

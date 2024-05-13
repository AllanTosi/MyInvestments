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
            public Guid ClasseAtivoId { get; set; }
            public string ClasseAtivoDescricao { get; set; }
            public Guid SetorId { get; set; }
            public string SetorDescricao { get; set; }
}

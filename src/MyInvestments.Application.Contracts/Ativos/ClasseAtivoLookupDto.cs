using System;
using Volo.Abp.Application.Dtos;

namespace MyInvestments.Ativos;

public class ClasseAtivoLookupDto : EntityDto<Guid>
{
    public string Nome { get; set; }
}

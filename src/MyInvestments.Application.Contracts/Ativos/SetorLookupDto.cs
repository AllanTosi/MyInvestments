using System;
using Volo.Abp.Application.Dtos;

namespace MyInvestments.Ativos;

public class SetorLookupDto : EntityDto<Guid>
{
    public string Descricao { get; set; }
}

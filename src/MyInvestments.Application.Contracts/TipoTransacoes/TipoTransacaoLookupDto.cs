using System;
using Volo.Abp.Application.Dtos;

namespace MyInvestments.TipoTransacoes;

public class TipoTransacaoLookupDto : EntityDto<Guid>
{
    public string Descricao { get; set; }
}

using System;
using Volo.Abp.Application.Dtos;

namespace MyInvestments.TipoTransacoes;

public class TipoTransacaoDto : EntityDto<Guid>
{
    public string Descricao { get; set; } = string.Empty;
}
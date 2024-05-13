using System;
using Volo.Abp.Application.Dtos;

namespace MyInvestments.Setores;

public class SetorDto : EntityDto<Guid>
{
    public string Descricao { get; set; } = string.Empty;
}
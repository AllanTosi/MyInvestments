using System;
using Volo.Abp.Application.Dtos;

namespace MyInvestments.ClasseAtivos;

public class ClasseAtivoDto : EntityDto<Guid>
{
    public string Nome { get; set; } = string.Empty;
}
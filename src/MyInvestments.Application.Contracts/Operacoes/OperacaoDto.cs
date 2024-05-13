﻿using System;
using Volo.Abp.Application.Dtos;

namespace MyInvestments.Operacoes;

public class OperacaoDto : EntityDto<Guid>
{
    public DateTime DataOperacao { get; set; }
    public int Quantidade { get; set; }
    public float Preco { get; set; }
    public float ValorEmulumento { get; set; }
    public float ValorIrpf { get; set; }
    public float ValorCorretagem { get; set; }
}
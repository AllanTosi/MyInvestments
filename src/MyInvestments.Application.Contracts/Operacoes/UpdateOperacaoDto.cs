using System;
using System.ComponentModel.DataAnnotations;

namespace MyInvestments.Operacoes;

public class UpdateOperacaoDto
{
    [Required]
    [DataType(DataType.Date)]
    public DateTime DataOperacao { get; set; }

    [Required]
    public int Quantidade { get; set; }

    [Required]
    public float Preco { get; set; }

    [Required]
    public float ValorEmulumento { get; set; }

    [Required]
    public float ValorIrpf { get; set; }

    public float? ValorCorretagem { get; set; } = 0;
}

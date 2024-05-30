using System;
using System.ComponentModel.DataAnnotations;

namespace MyInvestments.Operacoes;

public class UpdateOperacaoDto
{    
    [DataType(DataType.Date)]
    public DateTime DataOperacao { get; set; }
    
    public int Quantidade { get; set; }
    
    public float Preco { get; set; }
    
    public float ValorEmulumento { get; set; }

    public float ValorIrpf { get; set; }

    public float? ValorCorretagem { get; set; } = 0;

    //Adiciona referencias
    [Required]
    public Guid AtivoId { get; set; }
    [Required]
    public Guid TipoTransacaoId { get; set; }
}

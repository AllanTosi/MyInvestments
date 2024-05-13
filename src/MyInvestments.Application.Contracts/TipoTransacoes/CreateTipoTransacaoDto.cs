using System.ComponentModel.DataAnnotations;

namespace MyInvestments.TipoTransacoes;

public class CreateTipoTransacaoDto
{
    [Required]
    public string Descricao { get; set; } = string.Empty;
}
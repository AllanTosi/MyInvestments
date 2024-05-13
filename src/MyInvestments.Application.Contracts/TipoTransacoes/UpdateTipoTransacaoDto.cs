using System.ComponentModel.DataAnnotations;

namespace MyInvestments.TipoTransacoes;

public class UpdateTipoTransacaoDto
{
    [Required]
    public string Descricao { get; set; } = string.Empty;
}
using System.ComponentModel.DataAnnotations;

namespace MyInvestments.Setores;

public class UpdateSetorDto
{
    [Required]
    public string Descricao { get; set; } = string.Empty;
}
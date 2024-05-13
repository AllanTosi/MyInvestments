using System.ComponentModel.DataAnnotations;

namespace MyInvestments.Setores;

public class CreateSetorDto
{
    [Required]    
    public string Descricao { get; set; } = string.Empty;
}
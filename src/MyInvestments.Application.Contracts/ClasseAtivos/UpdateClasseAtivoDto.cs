using System.ComponentModel.DataAnnotations;

namespace MyInvestments.ClasseAtivos;

public class UpdateClasseAtivoDto
{
    [Required]
    [StringLength(200)]
    public string Nome { get; set; } = string.Empty;
}

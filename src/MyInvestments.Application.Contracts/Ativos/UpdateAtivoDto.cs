using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyInvestments.Ativos;

public class UpdateAtivoDto : IValidatableObject
{
    [Required]
    [StringLength(6)]
    public string Ticker { get; set; } = string.Empty;
    [Required]
    [StringLength(200)]
    public string Nome { get; set; } = string.Empty;

    // [StringLength(14)]
    // public int Cnpj { get; set; }

    public string? Descricao { get; set; }

                //Adiciona referencias
                public Guid ClasseAtivoId { get; set; }
                public Guid SetorId { get; set; }

    //Valida se nome e descrição são iguais
    public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
    {
        if (Nome == Descricao)
        {
            yield return new ValidationResult(
                "Nome e Descrição não podem ser iguais!",
                new[] { "Nome", "Descricao" }
            );
        }
    }
}

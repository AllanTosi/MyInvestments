using Volo.Abp.Application.Dtos;

namespace MyInvestments.ClasseAtivos;

public class GetClasseAtivoListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
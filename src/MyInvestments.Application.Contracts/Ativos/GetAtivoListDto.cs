using Volo.Abp.Application.Dtos;

namespace MyInvestments.Ativos;

public class GetAtivoListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
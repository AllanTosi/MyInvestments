using Volo.Abp.Application.Dtos;

namespace MyInvestments.Setores;

public class GetSetorListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
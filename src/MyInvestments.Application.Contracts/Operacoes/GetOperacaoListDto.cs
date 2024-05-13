using Volo.Abp.Application.Dtos;

namespace MyInvestments.Operacoes;

public class GetOperacaoListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
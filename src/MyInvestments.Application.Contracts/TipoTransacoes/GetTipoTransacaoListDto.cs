using Volo.Abp.Application.Dtos;

namespace MyInvestments.TipoTransacoes;

public class GetTipoTransacaoListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
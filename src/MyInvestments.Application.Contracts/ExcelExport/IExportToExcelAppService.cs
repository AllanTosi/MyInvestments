using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MyInvestments.ExcelExport
{
    public interface IExportToExcelAppService : IApplicationService
    {
        Task<byte[]> ExportToExcel();

        Task<byte[]> ExportToExcelAtivos();

        Task<byte[]> ExportToExcelTipoTransacoes();

        Task<byte[]> ExportToExcelSetores();

        Task<byte[]> ExportToExcelClasseAtivos();

        Task<byte[]> ExportToExcelOperacoes();

    }
}

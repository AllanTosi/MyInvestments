using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MyInvestments.ExcelExport
{
    public interface IExportToExcelAppService : IApplicationService
    {
        Task<byte[]> ExportToExcel();
    }
}

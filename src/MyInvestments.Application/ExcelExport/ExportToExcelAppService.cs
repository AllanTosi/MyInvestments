using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using static MyInvestments.ExcelExport.ExcelFileGenerator;

namespace MyInvestments.ExcelExport
{
    public class ExportToExcelAppService : ApplicationService, IExportToExcelAppService
    {
        public Task<byte[]> ExportToExcel() => Task.FromResult(GenerateExcelFile());
    }
}
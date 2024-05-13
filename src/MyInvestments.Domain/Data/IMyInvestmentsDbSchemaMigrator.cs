using System.Threading.Tasks;

namespace MyInvestments.Data;

public interface IMyInvestmentsDbSchemaMigrator
{
    Task MigrateAsync();
}

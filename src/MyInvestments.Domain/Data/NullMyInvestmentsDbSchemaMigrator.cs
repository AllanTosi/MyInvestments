using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace MyInvestments.Data;

/* This is used if database provider does't define
 * IMyInvestmentsDbSchemaMigrator implementation.
 */
public class NullMyInvestmentsDbSchemaMigrator : IMyInvestmentsDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}

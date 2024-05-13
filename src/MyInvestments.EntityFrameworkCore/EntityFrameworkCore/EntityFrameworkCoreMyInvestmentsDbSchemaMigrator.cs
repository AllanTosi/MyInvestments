using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyInvestments.Data;
using Volo.Abp.DependencyInjection;

namespace MyInvestments.EntityFrameworkCore;

public class EntityFrameworkCoreMyInvestmentsDbSchemaMigrator
    : IMyInvestmentsDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreMyInvestmentsDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the MyInvestmentsDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<MyInvestmentsDbContext>()
            .Database
            .MigrateAsync();
    }
}

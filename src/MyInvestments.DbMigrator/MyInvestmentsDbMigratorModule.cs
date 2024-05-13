using MyInvestments.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MyInvestments.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MyInvestmentsEntityFrameworkCoreModule),
    typeof(MyInvestmentsApplicationContractsModule)
    )]
public class MyInvestmentsDbMigratorModule : AbpModule
{
}

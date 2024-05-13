using Volo.Abp.Modularity;

namespace MyInvestments;

public abstract class MyInvestmentsApplicationTestBase<TStartupModule> : MyInvestmentsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

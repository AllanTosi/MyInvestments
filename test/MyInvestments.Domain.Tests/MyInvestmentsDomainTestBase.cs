using Volo.Abp.Modularity;

namespace MyInvestments;

/* Inherit from this class for your domain layer tests. */
public abstract class MyInvestmentsDomainTestBase<TStartupModule> : MyInvestmentsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

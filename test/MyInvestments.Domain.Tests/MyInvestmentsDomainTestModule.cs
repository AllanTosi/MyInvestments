using Volo.Abp.Modularity;

namespace MyInvestments;

[DependsOn(
    typeof(MyInvestmentsDomainModule),
    typeof(MyInvestmentsTestBaseModule)
)]
public class MyInvestmentsDomainTestModule : AbpModule
{

}

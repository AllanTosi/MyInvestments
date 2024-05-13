using Volo.Abp.Modularity;

namespace MyInvestments;

[DependsOn(
    typeof(MyInvestmentsApplicationModule),
    typeof(MyInvestmentsDomainTestModule)
)]
public class MyInvestmentsApplicationTestModule : AbpModule
{

}

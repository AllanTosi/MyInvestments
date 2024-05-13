using MyInvestments.Localization;
using Volo.Abp.AspNetCore.Components;

namespace MyInvestments.Blazor;

public abstract class MyInvestmentsComponentBase : AbpComponentBase
{
    protected MyInvestmentsComponentBase()
    {
        LocalizationResource = typeof(MyInvestmentsResource);
    }
}

using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace MyInvestments.Blazor;

[Dependency(ReplaceServices = true)]
public class MyInvestmentsBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "MyInvestments";
}

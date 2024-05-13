using Volo.Abp.Settings;

namespace MyInvestments.Settings;

public class MyInvestmentsSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(MyInvestmentsSettings.MySetting1));
    }
}

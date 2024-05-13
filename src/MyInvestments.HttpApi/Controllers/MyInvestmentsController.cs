using MyInvestments.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MyInvestments.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class MyInvestmentsController : AbpControllerBase
{
    protected MyInvestmentsController()
    {
        LocalizationResource = typeof(MyInvestmentsResource);
    }
}

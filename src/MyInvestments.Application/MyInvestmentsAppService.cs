using System;
using System.Collections.Generic;
using System.Text;
using MyInvestments.Localization;
using Volo.Abp.Application.Services;

namespace MyInvestments;

/* Inherit your application services from this class.
 */
public abstract class MyInvestmentsAppService : ApplicationService
{
    protected MyInvestmentsAppService()
    {
        LocalizationResource = typeof(MyInvestmentsResource);
    }
}

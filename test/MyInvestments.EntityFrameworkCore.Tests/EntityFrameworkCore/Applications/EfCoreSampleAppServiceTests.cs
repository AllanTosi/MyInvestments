using MyInvestments.Samples;
using Xunit;

namespace MyInvestments.EntityFrameworkCore.Applications;

[Collection(MyInvestmentsTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<MyInvestmentsEntityFrameworkCoreTestModule>
{

}

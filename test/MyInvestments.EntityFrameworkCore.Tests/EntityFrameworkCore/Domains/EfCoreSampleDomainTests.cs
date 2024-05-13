using MyInvestments.Samples;
using Xunit;

namespace MyInvestments.EntityFrameworkCore.Domains;

[Collection(MyInvestmentsTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<MyInvestmentsEntityFrameworkCoreTestModule>
{

}

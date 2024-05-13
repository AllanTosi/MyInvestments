using Xunit;

namespace MyInvestments.EntityFrameworkCore;

[CollectionDefinition(MyInvestmentsTestConsts.CollectionDefinitionName)]
public class MyInvestmentsEntityFrameworkCoreCollection : ICollectionFixture<MyInvestmentsEntityFrameworkCoreFixture>
{

}

using System.ComponentModel;
using Nito.Disposables;
using Xunit;
using XUnitTestProject1.Specs;

namespace locacao.tests.Internal
{
    public abstract class AbstractFixture<TStartupClass> : AbstractDisposable, IContextAwareFixture<TStartupClass>
        where TStartupClass: class, new()
    {
    }

    [CollectionDefinition(nameof(TestContext))]
    public class TestContextCollection : ICollectionFixture<TestContextFactory<TestStartup>> { }
}
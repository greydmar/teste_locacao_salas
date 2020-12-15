using System.ComponentModel;
using Nito.Disposables;
using Xunit;

namespace locacao.tests.Internal
{
    public abstract class AbstractFixture<TStartupClass> : AbstractDisposable, IContextAwareFixture<TStartupClass>
        where TStartupClass: class, new()
    {
    }

    [CollectionDefinition(nameof(TestContext))]
    public class TestContextCollection : ICollectionFixture<TestContextFactory<TestStartup>> { }
}
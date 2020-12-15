using Xunit;
using XUnitTestProject1.Specs;

namespace locacao.tests.Internal
{
    [Collection(nameof(TestContext))]
    public abstract class TesteBase: AbstractDisposable
    {
    }
}
using locacao.tests;
using locacao.tests.Internal;
using Xunit;

namespace XUnitTestProject1.Specs
{
    
    [Collection(nameof(TestContext))]
    public abstract class TesteBase: AbstractDisposable
    {
    }
}
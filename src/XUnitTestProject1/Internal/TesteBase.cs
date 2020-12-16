using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace mtgroup.locacao.Internal
{
    [Collection(nameof(TestContext))]
    public abstract class TesteBase: AbstractDisposable
    {
        protected async Task<TResult> UsingScoped<TResult>(Func<IServiceProvider, Task<TResult>> scopedAction)
        {
            using (var scoped = TestContext.Provider.CreateScope())
            {
                return await scopedAction(scoped.ServiceProvider);
            }
        }

        protected async Task UsingScoped(Func<IServiceProvider, Task> scopedAction)
        {
            using (var scoped = TestContext.Provider.CreateScope())
            {
                await scopedAction(scoped.ServiceProvider);
            }
        }
    }
}
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace mtgroup.locacao.Internal
{
    public abstract class TesteBaseController: IClassFixture<TesteWebApplicationFactory>
    {
        private readonly TesteWebApplicationFactory _factory;

        protected TesteWebApplicationFactory AppFactory => _factory;
        
        protected TesteBaseController(TesteWebApplicationFactory factory)
        {
            _factory = factory;
        }

        protected async Task<TResult> UsingClient<TResult>(Func<FlurlClient, Task<TResult>> action)
            where TResult: HttpResponseMessage
        {
            var options = new WebApplicationFactoryClientOptions();

            using (var httpClient = _factory.CreateClient(options))
            {
                var client = new FlurlClient(httpClient);

                return await action(client);
            }
        }

        protected async Task<TResult> DeserializeJson<TResult>(HttpResponseMessage message)
        {
            var responseData = await message.Content.ReadAsStreamAsync();

            return FlurlHttp.GlobalSettings
                .JsonSerializer.Deserialize<TResult>(responseData);
        }
    }
}
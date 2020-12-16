using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using mtgroup.locacao.Internal;
using Xunit;

namespace mtgroup.locacao.Testes
{
    public class TesteBasicoEndpoints: TesteBaseController
    {
        public TesteBasicoEndpoints(TesteWebApplicationFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/api/v1/auth")]
        [InlineData("/api/v1/auth/authenticate")]
        [InlineData("/api/v1/auth/agendamento")]
        [InlineData("/api/v1/auth/agendamento/agendar")]
        public async Task Teste_Endpoints(string url)
        {
            var client = AppFactory.CreateClient();

            var response = await client.PostAsync(url, null);

            response.Should()
                .NotHaveHttpStatusCode(HttpStatusCode.NotFound);
        }
    }
}


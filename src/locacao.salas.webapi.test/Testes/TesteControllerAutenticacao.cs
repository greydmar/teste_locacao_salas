using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using mtgroup.auth.Interfaces.Model;
using mtgroup.locacao.Controllers;
using mtgroup.locacao.Internal;
using Xunit;

namespace mtgroup.locacao.Testes
{
    public class TesteControllerAutenticacao : TesteBaseController
    {
        private Type _targetController;
        private ApiVersion _targetVersion;

        public TesteControllerAutenticacao(TesteWebApplicationFactory factory)
            : base(factory)
        {
            _targetController = typeof(AuthenticationController);
            _targetVersion = new ApiVersion(1, 0);
        }

        private string GetControllerRouteSegment()
        {
            return "api/v{version:apiVersion}/auth"
                .Replace("{version:apiVersion}", _targetVersion.ToString("V"));

        }

        protected async Task<HttpResponseMessage> PostAuthByCredentials(
            RequisicaoAutenticacaoUsuario requisicao)
        {
            return await UsingClient(async client =>
            {
                var routeSegment = GetControllerRouteSegment();
                var endpointUrlSegment = "authenticate";

                client
                    .AllowAnyHttpStatus();

                var request = client
                    .Request(routeSegment, endpointUrlSegment);

                return (await request.PostJsonAsync(requisicao))
                    .ResponseMessage;
            });
        }

        internal async Task<TResult> PostAndValidateAuthByCredentials<TResult>(
            RequisicaoAutenticacaoUsuario requisicao
        )
        {
            var uncheckedResponse = await PostAuthByCredentials(requisicao);

            uncheckedResponse.Should()
                .Be200Ok()
                .And
                .Satisfy<RespostaUsuarioAutorizado>(resposta =>
                {
                    resposta.TempoVidaToken.Should().BeGreaterThan(0);
                    resposta.Token.Should().NotBeEmpty();
                    resposta.NomeUsuario.Should().NotBeEmpty();
                });

            return await DeserializeJson<TResult>(uncheckedResponse);
        }

        [Fact]
        public async Task Acesso_Usuario_Nao_Cadastrado_Falha()
        {
            var requisicao = new RequisicaoAutenticacaoUsuario()
            {
                Login = "Usuario_Nao_Cadastrado",
                Senha = "Mudar@123"
            };
            
            var uncheckedResponse = await PostAuthByCredentials(requisicao);

            uncheckedResponse.Should()
                .Be401Unauthorized("Usuário não foi cadastrado na base");
        }

        [Fact]
        public async Task Acesso_Credenciais_Invalidas_Falha()
        {
            var requisicao = new RequisicaoAutenticacaoUsuario()
            {
                Login = "Usuario01",
                Senha = "Mudar@12345"
            };

            var uncheckedResponse = await PostAuthByCredentials(requisicao);

            uncheckedResponse.Should()
                .Be401Unauthorized("Credenciais inválidas");
        }

        [Fact]
        public async Task Acesso_Credenciais_Validas_Funciona()
        {
            var requisicao = new RequisicaoAutenticacaoUsuario()
            {
                Login = "Usuario01",
                Senha = "Mudar@123"
            };

            var response = await PostAndValidateAuthByCredentials<RespostaUsuarioAutorizado>(requisicao);
        }
    }
}
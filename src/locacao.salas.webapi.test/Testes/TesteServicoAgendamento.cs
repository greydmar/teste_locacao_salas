using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using mtgroup.auth.Interfaces.Model;
using mtgroup.locacao.Controllers;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Internal;
using mtgroup.locacao.Model;
using Nager.Date;
using Xunit;

namespace mtgroup.locacao.Testes
{
    public class TesteServicoAgendamento : TesteBaseController
    {
        private Type _targetController;
        private ApiVersion _targetVersion;

        public TesteServicoAgendamento(TesteWebApplicationFactory factory)
            : base(factory)
        {
            _targetController = typeof(ServicoAgendamentoController);
            _targetVersion = new ApiVersion(1, 0);
        }

        private string GetControllerRouteSegment()
        {
            return "api/v{version:apiVersion}/agendamento"
                .Replace("{version:apiVersion}", _targetVersion.ToString("V"));
        }

        protected async Task<HttpResponseMessage> PostAgendamento(
            RequisicaoAgendamento requisicao, RespostaUsuarioAutorizado autorizacao = null)
        {
            return await UsingClient(async client =>
            {
                var routeSegment = GetControllerRouteSegment();
                var endpointUrlSegment = "agendar";

                client
                    .AllowAnyHttpStatus();

                var request = client
                    .Request(routeSegment, endpointUrlSegment);
                
                if (autorizacao != null)
                    request.WithOAuthBearerToken(autorizacao.Token);

                return (await request.PostJsonAsync(requisicao))
                    .ResponseMessage;
            });
        }

        private async Task<RespostaUsuarioAutorizado> AssertAutenticacao(
            RequisicaoAutenticacaoUsuario reqUsuario
            )
        {
            using (new AssertionScope())
            {
                var caller = new TesteControllerAutenticacao(this.AppFactory);

                return await caller
                    .PostAndValidateAuthByCredentials<RespostaUsuarioAutorizado>(reqUsuario);
            }
        }

        internal async Task<TResult> PostAgendamentoComValidacao<TResult>(
            RequisicaoAgendamento requisicao,
            RespostaUsuarioAutorizado autorizacao
        )
        {
            var uncheckedResponse = await PostAgendamento(requisicao, autorizacao);

            uncheckedResponse.Should()
                .Be200Ok()
                .And
                .Satisfy<RespostaAgendamento>(resposta =>
                {
                    resposta.IdSalaReservada.Should().NotBeEmpty();
                });

            return await DeserializeJson<TResult>(uncheckedResponse);
        }

        [Fact]
        public async Task Chamada_Sem_Autenticacao_Recusada()
        {
            var requisicao = new RequisicaoAgendamento()
            {
                QuantidadePessoas = 5,
                AcessoInternet = false,
                DataInicio = DateTime.Now.Date.AddDays(2),
                DataFim = DateTime.Now.Date.AddDays(2),
                HoraFim = new TimeSpan(10, 50, 0),
                HoraInicio = new TimeSpan(13, 50, 0),
                TvWebCam = false
            };

            var uncheckedResponse = await PostAgendamento(requisicao, null);

            uncheckedResponse.Should()
                .Be401Unauthorized();
        }

        [Fact]
        public async Task Chamada_Com_Autenticacao_Eh_Aceita()
        {
            var reqAutenticacao = new RequisicaoAutenticacaoUsuario()
            {
                Login = "Usuario01",
                Senha = "Mudar@123"
            };

            var requisicao = new RequisicaoAgendamento()
            {
                QuantidadePessoas = 5,
                AcessoInternet = false,
                DataInicio = DateTime.Now.Date.AddDays(2),
                DataFim = DateTime.Now.Date.AddDays(2),
                HoraFim = new TimeSpan(10, 50, 0),
                HoraInicio = new TimeSpan(13, 50, 0),
                TvWebCam = false
            };

            var autorizacao = await AssertAutenticacao(reqAutenticacao);
            
            var uncheckedResponse = await PostAgendamento(requisicao, autorizacao);

            uncheckedResponse.Should()
                .NotHaveHttpStatusCode(HttpStatusCode.Unauthorized, "Autenticação é válida");
        }

        [Fact]
        public async Task Agendamento_Requisicao_Valida_Eh_Aceito()
        {
            var reqAutenticacao = new RequisicaoAutenticacaoUsuario()
            {
                Login = "Usuario01",
                Senha = "Mudar@123"
            };

            var dataReferencia = DateTime.Now.Date.AddDays(2);

            var listaAmostras = new ListaMinimaRequisicoesExigidas(dataReferencia);

            var autorizacao = await AssertAutenticacao(reqAutenticacao);

            using (new AssertionScope("teste requisitos mínimos"))
            {
                foreach (var objAmostra in listaAmostras)
                {
                    var amostra = objAmostra.First();
                    
                    var uncheckedResponse = await PostAgendamento((RequisicaoAgendamento)amostra, autorizacao);

                    uncheckedResponse.Should()
                        .Be200Ok();
                }
            }
        }
    }
}
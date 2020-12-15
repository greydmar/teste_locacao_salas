using System;
using System.Threading.Tasks;
using FluentResults;
using locacao.tests.DataContext;
using locacao.tests.Internal;
using Microsoft.Extensions.DependencyInjection;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces.Servicos;
using Xunit;

namespace locacao.tests.Testes
{
    [Collection(nameof(TestContext))]
    public class TestValidacaoRequisicao
    {
        private async Task<Result<RequisicaoSalaReuniao>> Validar(RequisicaoSalaReuniao requisicao)
        {
            using (var scoped = TestContext.Provider.CreateScope())
            {
                var servico = NsubstituteHelper.ServicoValidacao(scoped.ServiceProvider);
                var resultado = await servico.RequisicaoValida(requisicao);
                return resultado;
            }
        }
        
        [Fact]
        public async Task Data_Inicio_Deve_Ser_Hoje_Ou_Maior_Que_Hoje()
        {
            var requisicao = AuxiliarDados.
                Req_Uma_Sala(DateTime.Now.AddDays(-1), TimeSpan.FromHours(1), 15);

            var resultado = await Validar(requisicao);

            Assert.True(resultado.IsFailed);
        }

        [Fact]
        public async Task Quantidade_Pessoas_Nao_Deve_Exceder_Capacidade_Salas()
        {
            var requisicao = AuxiliarDados.
                Req_Uma_Sala(DateTime.Today, TimeSpan.FromHours(1), 21);

            var resultado = await Validar(requisicao);
            Assert.True(resultado.IsFailed);
        }

        [Fact]
        public async Task Tempo_Reuniao_Nao_Deve_Exceder_Oito_Horas()
        {
            var requisicao = AuxiliarDados.
                Req_Uma_Sala(DateTime.Today, TimeSpan.FromHours(9.5), 15);

            var resultado = await Validar(requisicao);
            Assert.True(resultado.IsFailed);
        }
    }
}

using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using mtgroup.locacao.DataContext;
using mtgroup.locacao.DataContext.EfContext;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Interfaces.Repositorios;
using mtgroup.locacao.Interfaces.Servicos;
using mtgroup.locacao.Internal;
using Xunit;

namespace mtgroup.locacao.Testes
{
    public class TestServicoAgendamento: TesteBase
    {
        public TestServicoAgendamento()
        {
        }

        [Fact]
        public async Task Nenhum_Agendamento_Registrado()
        {
            var periodo = new PeriodoLocacao(DateTime.Now.AddDays(60), DateTime.Now.AddDays(61));

            await UsingScoped(async provider =>
            {
                var salasDisponiveis = await provider.GetRequiredService<IRepositorioReservas>() 
                    .ListarSalasDisponiveis(periodo);

                salasDisponiveis.Count().Should()
                    .Be(AuxiliarDados.SalasDisponiveis.Count(),
                        "Nenhuma sala deve estar reservada acima de 40 dias ");
            });
        }

        [Fact]
        public async Task Inclusao_Agendamento_Base_Vazia_Funciona()
        {
            using(new AssertionScope())
            {
                // checagem de nenhum agendamento existente
                await Nenhum_Agendamento_Registrado();
            }

            // uma data/hora independente do momento em q o teste está sendo executado 
            var dataReferencia = DateTime.Now.Date
                .AddDays(2); /* critério de no mínimo 01 dia de antecedência */
            
            var proximoDiaUtil = DateSystemUtils.NearestWorkDateBetween(dataReferencia, dataReferencia.AddDays(38));

            var dataRef = DateTime.Now;
            
            var dataMarcacao = proximoDiaUtil
                .AddHours(08);

            var requisicao = new RequisicaoSalaReuniao(dataRef)
            {
                Periodo = new PeriodoLocacao(dataMarcacao, 02),
                QuantidadePessoas = 03,
                Recursos = RecursoSalaReuniao.Nenhum
            };

            var reserva = await UsingScoped(async provider =>
            {
                var svcAgendamento = provider.GetRequiredService<IServicoAgendamento>();

                return await svcAgendamento.EfetuarReserva(requisicao, false);
            });

            reserva.Should()
                .Match<ResultadoReservaSala>(sala => sala.IsSuccess, "Requisição atende aos parâmetros e todas as salas estão disponíveis");
        }

        [Theory(DisplayName="Testes mínimos exigidos")]
        [ClassData(typeof(ListaMinimaRequisicoesExigidas))]
        public async Task Verificao_Testes_Minimos(RequisicaoSalaReuniao requisicao)
        {
            var reserva = await UsingScoped(async provider =>
            {
                var svcAgendamento = provider.GetRequiredService<IServicoAgendamento>();

                return await svcAgendamento.EfetuarReserva(requisicao, false);
            });

            reserva.Should()
                .Match<ResultadoReservaSala>(sala => sala.IsSuccess, "Requisição atende aos parâmetros e todas as salas estão disponíveis");
        }

        //TODO: Teste de falha de inclusões com sugestões {requisicao valida, sala disponivel [recursos, maximo de 40 dias]}
        //TODO: Teste de falha de inclusões sem sugestões (requisicao invalida, requisicao valida mas sala indisponivel [recursos, maximo de 40 dias])
        //TODO: Teste de inclusões sucessivas {requisicoes validas e requisicoes invalidas. disparo sequencial}
        //TODO: Teste de inclusões paralelas {requisicoes validas e requisicoes invalidas. disparo sequencial}
    }
}

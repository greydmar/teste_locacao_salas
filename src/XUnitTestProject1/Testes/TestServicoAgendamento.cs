using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using locacao.tests.DataContext;
using locacao.tests.DataContext.EfContext;
using locacao.tests.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces.Servicos;
using Xunit;

namespace locacao.tests.Testes
{
    public class TestServicoAgendamento: TesteBase
    {
        internal TestContextoLocacaoSalas DbFixture { get; }
        
        public TestServicoAgendamento()
        {
            DbFixture = TestContext.Provider.GetRequiredService<TestContextoLocacaoSalas>();
        }


        private async Task RemoverTodosAgendamentos()
        {
            await DbFixture.Execute(async ctx =>
            {
                var dbSet = ctx.Set<ReservaSalaReuniao>();

                using (var allItems = dbSet.AsQueryable().GetEnumerator())
                {
                    while (allItems.MoveNext() && allItems.Current != null)
                        dbSet.Remove(allItems.Current);
                }

                await ctx.SaveChangesAsync(true);
            });
        }


        [Fact]
        public async Task Nenhum_Agendamento_Registrado()
        {
            await RemoverTodosAgendamentos();
            
            await DbFixture.Execute(async ctx =>
            {
                var dbSet = ctx.Set<ReservaSalaReuniao>();
                // Find any item
                var anyItem = await dbSet.AnyAsync();


                anyItem.Should().BeFalse("Não deve existir nenhum item armazenado");
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

            var dataRef = DateTime.Now;
            
            // uma data/hora independente do momento em q o teste está sendo executado 
            var dataMarcacao = DateTime.Now.Date
                .AddDays(2) /* critério de no mínimo 01 dia de antecedência */
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

        //TODO: Teste de falha de inclusões com sugestões {requisicao valida, sala disponivel [recursos, maximo de 40 dias]}
        //TODO: Teste de falha de inclusões sem sugestões (requisicao invalida, requisicao valida mas sala indisponivel [recursos, maximo de 40 dias])
        //TODO: Teste de inclusões sucessivas {requisicoes validas e requisicoes invalidas. disparo sequencial}
        //TODO: Teste de inclusões paralelas {requisicoes validas e requisicoes invalidas. disparo sequencial}
    }
}

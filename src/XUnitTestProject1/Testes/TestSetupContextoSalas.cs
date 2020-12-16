using System;
using System.Linq;
using mtgroup.locacao.DataContext;
using mtgroup.locacao.DataContext.EfContext;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Internal;
using Xunit;

namespace mtgroup.locacao.Testes
{
    /// <summary>
    /// Auxiliar de verificação do processo de montagem e configuração do mapeapeamento e do funcionamento do Contexto EF Core
    /// </summary>
    public class TesteContextoLocacaoSalas: TesteBase, IClassFixture<TestContextoLocacaoSalasSqlite>
    {
        public TesteContextoLocacaoSalas(TestContextoLocacaoSalasSqlite fixture)
        {
            DbFixture = fixture;
        }

        internal TestContextoLocacaoSalasSqlite DbFixture { get;  }

        private ContextoLocacaoSalas GerarContexto()
        {
            return DbFixture.CreateContext();
        }

        [Fact]
        public void Usuarios_Foram_Registradas()
        {
            var listaReferencia = AuxiliarDados.UsuariosAmostra.ToList();

            using (var ctx = GerarContexto())
            {
                var usuarios = ctx.ListaUsuarios.ToList();

                Assert.Contains(usuarios, interno =>
                    !string.IsNullOrEmpty(interno.Name) 
                    && listaReferencia.Any(item => item.Name == interno.Name)
                );
            }
        }

        [Fact]
        public void Salas_Foram_Registradas()
        {
            var listaReferencia = AuxiliarDados.SalasDisponiveis.ToList();
            
            using (var ctx = GerarContexto())
            {
                var salas = ctx.ListaSalas.ToList();

                Assert.Contains(salas, interno =>
                    interno.Id > 0
                    && listaReferencia.Any(item => item.Identificador == interno.Identificador)
                );
            }
        }

        [Fact]
        public void Inclusao_Reserva_Funciona()
        {
            var salaReferencia = AuxiliarDados.SalasDisponiveis
                .ToList()
                .FirstOrDefault();

            var solicitante = AuxiliarDados.UsuariosAmostra
                .First();

            var reserva = new ReservaSalaReuniao()
            {
                QuantidadePessoas = salaReferencia.QuantidadeAssentos,
                Periodo = new PeriodoLocacao(DateTime.Now, TimeSpan.FromHours(2)),
                IdSalaReservada = salaReferencia.Identificador,
                Solicitante = solicitante
            };
            
            using (var ctx = GerarContexto())
            {
                var dbset = ctx.Set<ReservaSalaReuniao>();
                var dbUser = ctx.ListaUsuarios.FirstOrDefault();

                reserva.Solicitante = dbUser;
                dbset.Add(reserva);

                ctx.SaveChanges();

                Assert.True(reserva.Id > 0);
                var saved = dbset.FirstOrDefault(c=> c.Id == reserva.Id );
                
                Assert.True(reserva.QuantidadePessoas == saved.QuantidadePessoas);
                Assert.True(reserva.Periodo == saved.Periodo);
                Assert.True(reserva.IdSalaReservada == saved.IdSalaReservada);
                Assert.True(!string.IsNullOrEmpty(reserva.CodigoReserva), "Codigo da Reserva não estar nulo");


                //Assert.Contains(salas, interno =>
                //    interno.Id > 0
                //    && listaReferencia.Any(item => item.Identificador == interno.Identificador)
                //);
            }
        }
    }
}
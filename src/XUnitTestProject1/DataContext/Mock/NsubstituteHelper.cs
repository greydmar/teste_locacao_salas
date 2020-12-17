using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Interfaces.Repositorios;
using mtgroup.locacao.Interfaces.Servicos;
using mtgroup.locacao.Servicos;
using NSubstitute;

namespace mtgroup.locacao.DataContext
{
    internal static class NsubstituteHelper
    {
        private static int _counter = 0;
        private static Dictionary<int, ReservaSalaReuniao> _cacheReservas = new Dictionary<int, ReservaSalaReuniao>();

        private static IEnumerable<IPerfilSalaReuniao> ListarSalasDisponiveis(PeriodoLocacao periodo)
        {
            var listaSalas = AuxiliarDados.SalasDisponiveis;

            lock (_cacheReservas)
            {
                var reservadas = _cacheReservas.Values
                    .Where(reserva => reserva.Periodo.Termino > periodo.Inicio)
                    .Select(reserva => reserva.IdSalaReservada);

                var dbgResult = listaSalas
                    .Where(sala => !reservadas.Contains(sala.Identificador))
                    .ToList();

                return dbgResult;
            }
        }

        public static IValidacaoRequisicao ServicoValidacao(IServiceProvider provider)
        {
            var svcDataHora = provider.GetRequiredService<IServicoDataHora>();
            var svcConsultas = ConsultaReservas(provider);

            return new ServicoValidacaoRequisicao(svcDataHora, svcConsultas);
        }

        private static IConsultaReservas SetupConsultaReservas(IConsultaReservas instance)
        {
            instance
                .ExistePerfilSala(Arg.Any<RequisicaoSalaReuniao>())
                .ReturnsForAnyArgs(callInfo =>
                {
                    var req = callInfo.Arg<RequisicaoSalaReuniao>();
                    return AuxiliarDados.ExistePerfilSala(req);
                });

            instance
                .ListarSalasDisponiveis(Arg.Any<PeriodoLocacao>())
                .ReturnsForAnyArgs(callInfo =>
                {
                    var req = callInfo.Arg<PeriodoLocacao>();
                    return ListarSalasDisponiveis(req);
                });
            
            return instance;
        }

        public static IConsultaReservas ConsultaReservas(IServiceProvider provider)
        {
            var instance = Substitute.For<IConsultaReservas>();

            return SetupConsultaReservas(instance);
        }


        public static IRepositorioReservas RepositorioReservas(IServiceProvider provider)
        {
            var instance = Substitute.For<IRepositorioReservas>();
            SetupConsultaReservas(instance);

            instance.GravarReserva(Arg.Any<ReservaSalaReuniao>())
                .ReturnsForAnyArgs(callInfo =>
                {
                    var req = callInfo.Arg<ReservaSalaReuniao>();

                    req.Id = Interlocked.Increment(ref _counter);
                    
                    lock (_cacheReservas)
                    {
                        _cacheReservas[req.Id] = req;
                    }

                    return req;
                });
            return instance;
        }
        
        public static IContextoExecucao ContextoExecucao(IServiceProvider arg)
        {
            var instance = Substitute.For<IContextoExecucao>();
            instance.Solicitante.Returns(AuxiliarDados.UsuariosAmostra.FirstOrDefault());

            return instance;
        }

    }
}

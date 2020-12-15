using System;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces.Repositorios;
using NSubstitute;

namespace locacao.tests.DataContext
{
    internal static class ConfiguracaoRepositorios
    {
        public static IConsultaReservas ConsultaReservas(IServiceProvider provider)
        {
            var instance = Substitute.For<IConsultaReservas>();

            instance
                .ExistePerfilSala(Arg.Any<RequisicaoSalaReuniao>())
                .ReturnsForAnyArgs(callInfo =>
                {
                    var req = callInfo.Arg<RequisicaoSalaReuniao>();
                    return AuxiliarDados.ExistePerfilSala(req);
                });
            
            return instance;
        }
    }
}

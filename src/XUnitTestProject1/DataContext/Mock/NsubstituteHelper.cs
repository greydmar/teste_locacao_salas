using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Interfaces.Repositorios;
using mtgroup.locacao.Interfaces.Servicos;
using mtgroup.locacao.Servicos;
using NSubstitute;

namespace locacao.tests.DataContext
{
    internal static class NsubstituteHelper
    {
        public static IValidacaoRequisicao ServicoValidacao(IServiceProvider provider)
        {
            var svcDataHora = provider.GetRequiredService<IServicoDataHora>();
            var svcConsultas = ConsultaReservas(provider);

            return new ServicoValidacaoRequisicao(svcDataHora, svcConsultas);
        }
        
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

        public static IContextoExecucao ContextoExecucao(IServiceProvider arg)
        {
            var instance = Substitute.For<IContextoExecucao>();
            instance.Solicitante.Returns(AuxiliarDados.UsuariosAmostra.FirstOrDefault());

            return instance;
        }

    }
}

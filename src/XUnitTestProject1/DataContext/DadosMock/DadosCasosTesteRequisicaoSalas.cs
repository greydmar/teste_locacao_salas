using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;

namespace locacao.tests.DadosMock
{
    internal static class DadosCasosTesteRequisicaoSalas
    {
        internal static Lazy<IEnumerable<IPerfilSalaReuniao>> SalasDisponiveis
        {
            get
            {
                return new Lazy<IEnumerable<IPerfilSalaReuniao>>(() =>
                    new[]
                    {
                        PerfisDisponiveis.PerfilSala01,
                        PerfisDisponiveis.PerfilSala02,
                        PerfisDisponiveis.PerfilSala03,
                        PerfisDisponiveis.PerfilSala04
                    }
                );
            }
        }

        internal static Lazy<IEnumerable<RequisicaoSalaReuniao>> Requisicoes
        {
            get
            {
                return new Lazy<IEnumerable<RequisicaoSalaReuniao>>(() =>
                    BogusHelper.Gerador(new DateTime(2020, 12, 10), 100));
            }
        }
        
        public static RequisicaoSalaReuniao Uma_Sala_Para(
            DateTime dataInicio, TimeSpan duracao, int participantes, 
            RecursoSalaReuniao recursos = RecursoSalaReuniao.Nenhum)
        {
            return new RequisicaoSalaReuniao(dataInicio)
            {
                Periodo = new PeriodoLocacao(dataInicio, duracao),
                QuantidadePessoas = Convert.ToUInt16(participantes),
                Recursos = recursos
            };
        }

        public static RequisicaoSalaReuniao Uma_Requisicao_Sala_Que_Atenda(
            int participantes, RecursoSalaReuniao recursos = RecursoSalaReuniao.Nenhum)
        {
            return Requisicoes.Value
                .FirstOrDefault(
                    req => req.QuantidadePessoas == participantes
                           && (req.Recursos & recursos) != 0
                );
        }
    }

    internal static class AuxiliarDados
    {
        public static TimeSpan Horas(int numeroHoras)
        {
            return TimeSpan.FromHours(numeroHoras);
        }

        public static RequisicaoSalaReuniao Req_Uma_Sala(DateTime dataInicio, TimeSpan duracao, int numParticipantes)
        {
            return DadosCasosTesteRequisicaoSalas
                .Uma_Sala_Para(dataInicio, duracao, numParticipantes);
        }

        public static RequisicaoSalaReuniao Req_Uma_Sala_Hoje(TimeSpan duracao, int numParticipantes)
        {
            return DadosCasosTesteRequisicaoSalas
                .Uma_Sala_Para(DateTime.Today, duracao, numParticipantes);
        }

        public static bool ExistePerfilSala(RequisicaoSalaReuniao req)
        {
            var listaSalas = DadosCasosTesteRequisicaoSalas.SalasDisponiveis.Value;

            return listaSalas.Any(sala =>
            {
                if (sala.QuantidadeAssentos < req.QuantidadePessoas)
                    return false;

                if (req.Recursos == RecursoSalaReuniao.Nenhum)
                    return true;

                return (sala.Recursos & req.Recursos) != 0;
            });
        }
    }
}

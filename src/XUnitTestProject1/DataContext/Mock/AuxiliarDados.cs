using System;
using System.Collections.Generic;
using System.Linq;
using locacao.clientebd.DTO;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;

namespace locacao.tests.DataContext
{
    internal static class AuxiliarDados
    {
        private static readonly Lazy<IEnumerable<IPerfilSalaReuniao>> _lzySalas = 
            new Lazy<IEnumerable<IPerfilSalaReuniao>>(() =>
            new[]
            {
                PerfisDisponiveis.PerfilSala01,
                PerfisDisponiveis.PerfilSala02,
                PerfisDisponiveis.PerfilSala03,
                PerfisDisponiveis.PerfilSala04
            }
        );

        private static readonly Lazy<IEnumerable<RequisicaoSalaReuniao>> _lzyRequisicoes =
            new Lazy<IEnumerable<RequisicaoSalaReuniao>>(() =>
                BogusHelper.Gerador(new DateTime(2020, 12, 10), 100)
            );
        
        private static readonly Lazy<IEnumerable<Solicitante>> _lzyUsuarios =
            new Lazy<IEnumerable<Solicitante>>(() =>
                new[]
                {
                    new Solicitante(){Name = "Usuario01"},
                    new Solicitante(){Name = "Usuario02"},
                    new Solicitante(){Name = "Usuario03"}
                }
            );

        public static IEnumerable<IPerfilSalaReuniao> SalasDisponiveis
        {
            get
            {
                var salas = new IPerfilSalaReuniao[] { }
                    .Concat(Enumerable.Repeat(PerfisDisponiveis.PerfilSala01, 5))
                    .Concat(Enumerable.Repeat(PerfisDisponiveis.PerfilSala02, 2))
                    .Concat(Enumerable.Repeat(PerfisDisponiveis.PerfilSala03, 3))
                    .Concat(Enumerable.Repeat(PerfisDisponiveis.PerfilSala04, 2))
                    .Select((sala, idx)=>
                    {
                        var result = new PerfilSalaReuniaoInterno(sala)
                        {
                            Identificador = (idx+1).ToString("D2")
                        };
                        return result;
                    });

                return salas.ToList();

            }
        }

        internal static IEnumerable<IPerfilSalaReuniao> PerfisSalasDisponiveis => _lzySalas.Value;

        internal static IEnumerable<Solicitante> UsuariosAmostra => _lzyUsuarios.Value;

        internal static IEnumerable<RequisicaoSalaReuniao> Requisicoes => _lzyRequisicoes.Value;

        public static RequisicaoSalaReuniao Uma_Requisicao_Sala_Que_Atenda(
            int participantes, RecursoSalaReuniao recursos = RecursoSalaReuniao.Nenhum)
        {
            return Requisicoes
                .FirstOrDefault(
                    req => req.QuantidadePessoas == participantes
                           && (req.Recursos & recursos) != 0
                );
        }
        
        public static RequisicaoSalaReuniao Req_Uma_Sala(
            DateTime dataInicio, TimeSpan duracao, int numParticipantes
            ,RecursoSalaReuniao recursos = RecursoSalaReuniao.Nenhum)
        {
            return new RequisicaoSalaReuniao(dataInicio)
            {
                Periodo = new PeriodoLocacao(dataInicio, duracao),
                QuantidadePessoas = Convert.ToUInt16(numParticipantes),
                Recursos = recursos
            };
        }

        public static RequisicaoSalaReuniao Req_Uma_Sala_Hoje(TimeSpan duracao, int numParticipantes)
        {
            return Req_Uma_Sala(DateTime.Today, duracao, numParticipantes, RecursoSalaReuniao.Nenhum);
        }

        public static bool ExistePerfilSala(RequisicaoSalaReuniao req)
        {
            var listaSalas = PerfisSalasDisponiveis;

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
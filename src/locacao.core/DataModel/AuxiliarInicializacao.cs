using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace mtgroup.locacao.DataModel
{
    internal static class AuxiliarInicializacao
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

        public static IEnumerable<IPerfilSalaReuniao> PerfisSalasDisponiveis => _lzySalas.Value;

        public static IEnumerable<IPerfilSalaReuniao> SalasDisponiveis
        {
            get
            {
                var salas = new IPerfilSalaReuniao[] { }
                    .Concat(Enumerable.Repeat(PerfisDisponiveis.PerfilSala01, 5))
                    .Concat(Enumerable.Repeat(PerfisDisponiveis.PerfilSala02, 2))
                    .Concat(Enumerable.Repeat(PerfisDisponiveis.PerfilSala03, 3))
                    .Concat(Enumerable.Repeat(PerfisDisponiveis.PerfilSala04, 2))
                    .Select((perfilSala, idx)=>
                    {
                        var result = new PerfilSalaReuniao(idx+1)
                        {
                            Identificador = (idx+1).ToString("D2"),
                            Grupo = perfilSala.Grupo,
                            QuantidadeAssentos = perfilSala.QuantidadeAssentos,
                            Recursos = perfilSala.Recursos
                        };
                        return result;
                    });

                return salas.ToImmutableList();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace mtgroup.locacao.DataModel
{
    internal sealed class AuxiliarReservas
    {
        public static IPerfilSalaReuniao EncontrarSalaCompativel(
            IEnumerable<IPerfilSalaReuniao> salas,
            RequisicaoSalaReuniao criterio)
        {
            var candidatas = EncontrarSalasCompativeis(salas, criterio);

            return candidatas
                .FirstOrDefault();
        }

        public static IEnumerable<IPerfilSalaReuniao> EncontrarSalasCompativeis(
            IEnumerable<IPerfilSalaReuniao> salas,
            RequisicaoSalaReuniao criterio)
        {
            return salas
                    .Select(perfil => new
                    {
                        perfil,
                        perfil.Recursos,
                        CapacidadeExcedente = (perfil.QuantidadeAssentos - criterio.QuantidadePessoas)
                        /*RecursosExcedentes = Convert.ToInt32(perfil.Recursos - criterio.Recursos)*/
                    })
                    .Where(item => item.CapacidadeExcedente >= 0 && item.perfil.Atende(criterio.Recursos))
                    .OrderBy(item => (item.CapacidadeExcedente/*, item.RecursosExcedentes*/))
                    .Select(item=>item.perfil)
                /*.ToList()*/;
        }
    }
}
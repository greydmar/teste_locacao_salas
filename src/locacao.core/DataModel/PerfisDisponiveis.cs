using System.Collections.Generic;
using mtgroup.locacao.Interfaces;

namespace mtgroup.locacao.DataModel
{
    
    /// <summary>
    /// Categorias de salas disponíveis (por recursos e capacidade)
    /// </summary>
    public static class PerfisDisponiveis
    {
        public static IPerfilSalaReuniao PerfilSala01 = new PerfilSalaReuniao()
        {
            Grupo = "Grupo01",
            QuantidadeAssentos = 10,
            Recursos = RecursoSalaReuniao.VideoConferencia
        };

        public static IPerfilSalaReuniao PerfilSala02 = new PerfilSalaReuniao()
        {
            Grupo = "Grupo02",
            QuantidadeAssentos = 10,
            Recursos = RecursoSalaReuniao.AcessoInternet
        };
        
        public static IPerfilSalaReuniao PerfilSala03 = new PerfilSalaReuniao()
        {
            Grupo = "Grupo03",
            QuantidadeAssentos = 03,
            Recursos = RecursoSalaReuniao.AcessoInternet|RecursoSalaReuniao.Computador
        };

        public static IPerfilSalaReuniao PerfilSala04 = new PerfilSalaReuniao()
        {
            Grupo = "Grupo04",
            QuantidadeAssentos = 20,
            Recursos = RecursoSalaReuniao.Nenhum
        };
    }
}
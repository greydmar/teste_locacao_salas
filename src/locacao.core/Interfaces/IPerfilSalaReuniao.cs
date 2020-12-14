using mtgroup.locacao.DataModel;

namespace mtgroup.locacao.Interfaces
{
    public interface IPerfilSalaReuniao
    {
        public string Identificador { get; }

        public string Nome { get; }

        public ushort QuantidadeAssentos { get; }

        public RecursoSalaReuniao Recursos { get; }
    }

    internal class PerfilSalaReuniao: IPerfilSalaReuniao
    {
        public string Identificador { get; set; }
        public string Nome { get; set; }
        public ushort QuantidadeAssentos { get; set; }
        public RecursoSalaReuniao Recursos { get; set; }
    }
    
    public static class PerfisDisponiveis
    {
        public static IPerfilSalaReuniao PerfilSala01 = new PerfilSalaReuniao()
        {
            QuantidadeAssentos = 10,
            Recursos = RecursoSalaReuniao.VideoConferencia
        };

        public static IPerfilSalaReuniao PerfilSala02 = new PerfilSalaReuniao()
        {
            QuantidadeAssentos = 10,
            Recursos = RecursoSalaReuniao.AcessoInternet
        };
        
        public static IPerfilSalaReuniao PerfilSala03 = new PerfilSalaReuniao()
        {
            QuantidadeAssentos = 03,
            Recursos = RecursoSalaReuniao.AcessoInternet|RecursoSalaReuniao.Computador
        };

        public static IPerfilSalaReuniao PerfilSala04 = new PerfilSalaReuniao()
        {
            QuantidadeAssentos = 20,
            Recursos = RecursoSalaReuniao.Nenhum
        };
    }
}
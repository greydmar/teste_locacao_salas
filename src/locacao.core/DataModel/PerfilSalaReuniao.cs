using mtgroup.locacao.Interfaces;

namespace mtgroup.locacao.DataModel
{
    internal class PerfilSalaReuniao: IPerfilSalaReuniao
    {
        public string Grupo { get; set; }
        
        public string Identificador { get; set; }
        
        public ushort QuantidadeAssentos { get; set; }
        
        public RecursoSalaReuniao Recursos { get; set; }
    }
}
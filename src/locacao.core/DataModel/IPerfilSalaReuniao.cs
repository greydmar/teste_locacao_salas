namespace mtgroup.locacao.DataModel
{
    public interface IPerfilSalaReuniao
    {
        public string Grupo { get; }
        
        public string Identificador { get; }

        public ushort QuantidadeAssentos { get; }

        public RecursoSalaReuniao Recursos { get; }
    }
    
    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;

namespace locacao.clientebd.DTO
{
    public class PerfilSalaReuniaoInterno: IEntidade, IPerfilSalaReuniao
    {
        public PerfilSalaReuniaoInterno()
        {

        }
        
        public PerfilSalaReuniaoInterno(IPerfilSalaReuniao sala)
        {
            this.Grupo = sala.Grupo;
            this.Identificador = sala.Identificador;
            this.Recursos = sala.Recursos;
            this.QuantidadeAssentos = sala.QuantidadeAssentos;
        }

        public int Id { get; set; }
        public string Grupo { get; set; }
        public string Identificador { get; set; }
        public ushort QuantidadeAssentos { get; set; }
        public RecursoSalaReuniao Recursos { get; set; }

        //public static PerfilSalaReuniaoInterno From(IPerfilSalaReuniao template)
        //{
        //    return new PerfilSalaReuniaoInterno(template);
        //}
    }
}

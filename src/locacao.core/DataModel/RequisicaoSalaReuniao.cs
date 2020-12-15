using System;
using System.Text;
using mtgroup.locacao.Interfaces;

namespace mtgroup.locacao.DataModel
{
    /// <summary>
    /// Requisição de alocação de <see cref="IPerfilSalaReuniao"/>
    /// </summary>
    public class RequisicaoSalaReuniao
    {
        public RequisicaoSalaReuniao()
        {
            DataRegistro = DateTime.Now;
        }

        public RequisicaoSalaReuniao(DateTime dataRegistro)
        {
            DataRegistro = dataRegistro;
        }

        public DateTime DataRegistro { get;  }

        public PeriodoLocacao Periodo { get; set; }

        public ushort QuantidadePessoas { get; set; }

        public RecursoSalaReuniao Recursos { get; set; }

        public string DescricaoPerfil()
        {
            var sb = new StringBuilder("Perfil= {");
            sb.AppendFormat("Periodo_Solicitado=[{0:D}, {1:G} horas]", this.Periodo.Inicio, this.Periodo.Horas);
            sb.AppendFormat("QuantidadePessoas={0:D}", this.QuantidadePessoas);
            sb.AppendFormat(",Recursos=[{0:F}]", Recursos);
            sb.Append('}');
            return sb.ToString();
        }
    }
}
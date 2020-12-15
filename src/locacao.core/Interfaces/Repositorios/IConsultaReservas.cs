using System.Collections.Generic;
using System.Threading.Tasks;
using mtgroup.locacao.DataModel;

namespace mtgroup.locacao.Interfaces.Repositorios
{
    public interface IConsultaReservas
    {
        Task<bool> ExistePerfilSala(RequisicaoSalaReuniao requisicao);
        
        Task<IEnumerable<IPerfilSalaReuniao>> ListarSalasDisponiveis(PeriodoLocacao periodo);
    }
}
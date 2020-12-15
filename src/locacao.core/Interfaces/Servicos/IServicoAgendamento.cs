using System.Collections.Generic;
using System.Threading.Tasks;
using mtgroup.locacao.DataModel;

namespace mtgroup.locacao.Interfaces.Servicos
{
    public interface IServicoAgendamento
    {
        Task<ResultadoReservaSala> EfetuarReserva(RequisicaoSalaReuniao requisicao, bool gerarSugestoes = true);

        Task<IEnumerable<IPerfilSalaReuniao>> ListarSugestoes(RequisicaoSalaReuniao requisicao);
    }
}
using System;
using System.Threading.Tasks;
using mtgroup.locacao.DataModel;

namespace mtgroup.locacao.Servicos
{
    public interface IServicoAgendamento
    {
        Task<ResultadoReservaSala> EfetuarReserva(RequisicaoSalaReuniao requisicao, bool gerarSugestoes = true);

        Task<ResultadoReservaSala> ListarSugestoes(RequisicaoSalaReuniao requisicao);
    }

    internal class ServicoAgendamento: IServicoAgendamento
    {
        public async Task<ResultadoReservaSala> EfetuarReserva(RequisicaoSalaReuniao requisicao, bool gerarSugestoes = true)
        {
            throw new NotImplementedException();

        }

        public async Task<ResultadoReservaSala> ListarSugestoes(RequisicaoSalaReuniao requisicao)
        {
            throw new NotImplementedException();
        }
    }
}
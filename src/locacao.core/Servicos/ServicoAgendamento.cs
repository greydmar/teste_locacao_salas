using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Interfaces.Repositorios;
using mtgroup.locacao.Interfaces.Servicos;

namespace mtgroup.locacao.Servicos
{
    internal class ServicoAgendamento: IServicoAgendamento
    {
        private readonly IRepositorioReservas _ctxReservas;
        private readonly IValidacaoRequisicao _reqValidacao;

        public ServicoAgendamento(
            IRepositorioReservas ctxReservas,
            IValidacaoRequisicao validacao
        )
        {
            _ctxReservas = ctxReservas;
            _reqValidacao = validacao;
        }

        private ReservaSalaReuniao NovaReserva(IEnumerable<IPerfilSalaReuniao> salas, RequisicaoSalaReuniao requisicao)
        {
            var sala = AuxiliarReservas.EncontrarSalaCompativel(salas, requisicao);
            
            var result = new ReservaSalaReuniao
            {
                Periodo = requisicao.Periodo,
                QuantidadePessoas = requisicao.QuantidadePessoas,
                IdSalaReservada = sala.Identificador
            };

            return result;
        }
        
        public async Task<ResultadoReservaSala> EfetuarReserva(RequisicaoSalaReuniao requisicao, bool gerarSugestoes = true)
        {
            if (requisicao == null)
                throw new ArgumentNullException(nameof(requisicao));

            var reqValida = await _reqValidacao.RequisicaoValida(requisicao);

            bool deveSugerir = false;
            
            if (reqValida.IsFailed)
            {
                deveSugerir = (gerarSugestoes && _reqValidacao.EhPossivelSugerir(reqValida, requisicao));
                
                if (!deveSugerir)
                    return ResultadoReservaSala.Falhou(reqValida, "Tentativa de reserva falhou.");
                
                return ResultadoReservaSala
                    .FalhouComSugestoes(await ListarSugestoes(requisicao));
            }

            var reqDisponibilidade = await _ctxReservas.ListarSalasDisponiveis(requisicao.Periodo);
            
            var reservaSala = NovaReserva(reqDisponibilidade, requisicao);

            reservaSala = await _ctxReservas
                .GravarReserva(reservaSala);
            
            return ResultadoReservaSala.
                OK(reservaSala);
        }

        public async Task<IEnumerable<IPerfilSalaReuniao>> ListarSugestoes(RequisicaoSalaReuniao requisicao)
        {
            return (await _ctxReservas.ListarSalasDisponiveis(requisicao.Periodo))
                .Take(03);
        }
    }
}
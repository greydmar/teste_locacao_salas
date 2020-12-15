using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Interfaces.Repositorios;
using mtgroup.locacao.Interfaces.Servicos;

namespace mtgroup.locacao.Servicos
{
    public class ServicoAgendamento: IServicoAgendamento
    {
        private readonly IContextoExecucao _ctxExecucao;
        private readonly IRepositorioReservas _ctxReservas;
        private readonly IValidacaoRequisicao _reqValidacao;

        public ServicoAgendamento(
            IContextoExecucao ctxExecucao,
            IRepositorioReservas ctxReservas,
            IValidacaoRequisicao validacao
        )
        {
            _ctxExecucao = ctxExecucao;
            _ctxReservas = ctxReservas;
            _reqValidacao = validacao;
        }

        private ReservaSalaReuniao NovaReserva(IEnumerable<IPerfilSalaReuniao> salas, RequisicaoSalaReuniao requisicao)
        {
            var sala = AuxiliarReservas.EncontrarSalaCompativel(salas, requisicao);
            
            var result = new ReservaSalaReuniao
            {
                Periodo = requisicao.Periodo,
                Solicitante = _ctxExecucao.Solicitante,
                QuantidadePessoas = requisicao.QuantidadePessoas,
                IdSalaReservada = sala.Identificador
            };

            return result;
        }
        
        public async Task<ResultadoReservaSala> EfetuarReserva(RequisicaoSalaReuniao requisicao, bool gerarSugestoes = true)
        {
            if (requisicao == null)
                throw new ArgumentNullException(nameof(requisicao));

            var tmpReq = await _reqValidacao.RequisicaoValida(requisicao);

            // Neste ponto, uma requisição inválida deve ser corrigida.
            // Não incluiremos sugestões
            if (tmpReq.IsFailed)
                return ResultadoReservaSala.Falhou(tmpReq, "Tentativa de reserva falhou.");

            var salasDisponiveis = await _ctxReservas.ListarSalasDisponiveis(requisicao.Periodo);
            
            // Deste ponto em diante é possível fazer sugestões
            if (!salasDisponiveis.Any())
            {
                return await GerarSugestoesSeNecessario(tmpReq, requisicao, gerarSugestoes, "Tentativa de reserva falhou. Nenhuma sala disponível");
            }
           
            var reservaSala = NovaReserva(salasDisponiveis, requisicao);

            var resultado = await Result.Try(
                async () => await _ctxReservas.GravarReserva(reservaSala)
            );

            if (resultado.IsFailed)
                return await GerarSugestoesSeNecessario(resultado, requisicao, gerarSugestoes, "Falha na tentativa de gravar reserva.");
            
            return ResultadoReservaSala.OK(reservaSala);
        }

        private async Task<ResultadoReservaSala> GerarSugestoesSeNecessario(Result falha,
            RequisicaoSalaReuniao requisicao, bool gerarSugestoes, string mensagemErro)
        {
            if (!gerarSugestoes)
                return ResultadoReservaSala.Falhou(falha, mensagemErro);

            return ResultadoReservaSala
                .FalhouComSugestoes(falha, await ListarSugestoes(requisicao));
        }

        public async Task<IEnumerable<IPerfilSalaReuniao>> ListarSugestoes(RequisicaoSalaReuniao requisicao)
        {
            var salasPeriodo = await _ctxReservas.ListarSalasDisponiveis(requisicao.Periodo);
            var salas = AuxiliarReservas.EncontrarSalasCompativeis(salasPeriodo, requisicao);
            
            return salas.Take(03);
        }
    }
}
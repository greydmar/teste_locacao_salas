﻿using System;
using System.Linq;
using System.Threading.Tasks;
using locacao.auth.core.Servicos;
using Microsoft.AspNetCore.Mvc;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces.Servicos;
using mtgroup.locacaosalas.Auxiliares;
using mtgroup.locacaosalas.Model;

namespace mtgroup.locacaosalas.Controllers
{
    using static Microsoft.AspNetCore.Http.StatusCodes;

    [ApiController]
    [ApiVersion("1.0")]
    [MtGroupAuthorize]
    [Route("api/v{version:apiVersion}/agendamento")]
    public class ServicoAgendamentoController : ControllerBase
    {
        private readonly IServicoAgendamento _svcAgendamento;

        public ServicoAgendamentoController(IServicoAgendamento svcAgendamento)
        {
            _svcAgendamento = svcAgendamento;
        }

        [HttpPost("agendar")]
        [Produces("application/json")]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaAgendamento), Status200OK)]
        public async Task<IActionResult> AgendarSala(RequisicaoAgendamento requisicao)
        {
            var reqTraduzida = Traduzir(requisicao);
            
            var resposta = await _svcAgendamento.EfetuarReserva(reqTraduzida);

            if (resposta.IsSuccess)
            {
                var agendamento = Traduzir(resposta.Value);
                return Ok(agendamento);
            }

            ////var nAutorizado = resposta.Errors.OfType<UsuarioNaoAutorizado>();
            ////if (nAutorizado.Any())
            ////{
            ////    return Unauthorized(nAutorizado.FirstOrDefault());
            ////}

            var outros = resposta.Errors;

            if (outros.Any())
                return BadRequest(outros.FirstOrDefault());

            return StatusCode(Status500InternalServerError);
        }

        private RespostaAgendamento Traduzir(ReservaSalaReuniao resposta)
        {
            return new RespostaAgendamento()
            {
                IdSalaReservada = "SALA " + resposta.IdSalaReservada
            };
        }
        
        private RequisicaoSalaReuniao Traduzir(RequisicaoAgendamento requisicao)
        {
            var dtInicio = requisicao.DataInicio.Date.Add(requisicao.HoraInicio);
            var dtFim = requisicao.DataFim.Date.Add(requisicao.HoraFim);

            var tmpData = dtInicio > dtFim ? dtFim : dtInicio;
            dtFim = dtFim > tmpData ? tmpData : dtFim;
            dtInicio = tmpData;

            var recursos = RecursoSalaReuniao.Nenhum;
            if (requisicao.AcessoInternet)
                recursos |= RecursoSalaReuniao.AcessoInternet;
            if (requisicao.TvWebCam)
                recursos |= RecursoSalaReuniao.VideoConferencia;

            var result = new RequisicaoSalaReuniao(DateTime.Now)
            {
                QuantidadePessoas = Convert.ToUInt16(requisicao.QuantidadePessoas),
                Periodo = new PeriodoLocacao(dtInicio, dtFim - dtInicio),
                Recursos = recursos
            };

            return result;
        }
    }
}
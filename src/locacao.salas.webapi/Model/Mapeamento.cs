using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mtgroup.locacao.DataModel;

namespace mtgroup.locacao.Model
{
    public sealed class Mapeamento
    {
        public static RespostaAgendamento Mapear(ReservaSalaReuniao resposta)
        {
            return new RespostaAgendamento()
            {
                IdSalaReservada = "SALA " + resposta.IdSalaReservada
            };
        }

        public static RequisicaoSalaReuniao Mapear(RequisicaoAgendamento requisicao)
        {
            var dtInicio = requisicao.DataInicio.Date.Add(requisicao.HoraInicio);
            var dtFim = requisicao.DataFim.Date.Add(requisicao.HoraFim);

            var tmpData = dtInicio;
            dtInicio = dtFim < tmpData ? dtFim : tmpData;
            dtFim = dtFim > tmpData ? dtFim : tmpData;
            

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

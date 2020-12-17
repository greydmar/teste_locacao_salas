using System;
using System.Collections.Generic;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Internal;
using mtgroup.locacao.Model;

namespace mtgroup.locacao.Testes
{
    public class ListaMinimaRequisicoesExigidas: XUnitTheoryClassData<RequisicaoAgendamento>
    {
        private readonly DateTime _dataReferencia;

        public ListaMinimaRequisicoesExigidas(in DateTime dataReferencia)
        {
            _dataReferencia = dataReferencia;
        }

        protected override IEnumerable<RequisicaoAgendamento> GetEnumerableData()
        {
            // uma data/hora independente do momento em q o teste está sendo executado 
            var dataInicio = DateSystemUtils
                .NearestWorkDateBetween(_dataReferencia, _dataReferencia.AddDays(38));

            yield return new RequisicaoAgendamento
            {
                QuantidadePessoas = 10,
                DataInicio = dataInicio, /* 10:00 da manhã, 02 horas reunião*/
                HoraInicio = TimeSpan.FromHours(10),
                DataFim = dataInicio,
                HoraFim = TimeSpan.FromHours(12),
                AcessoInternet = true,
                TvWebCam = true
            };

            yield return new RequisicaoAgendamento
            {
                QuantidadePessoas = 10,
                DataInicio = dataInicio, /* 10:00 da manhã, 02 horas reunião*/
                HoraInicio = TimeSpan.FromHours(10),
                DataFim = dataInicio,
                HoraFim = TimeSpan.FromHours(12),
                AcessoInternet = true,
                TvWebCam = true
            };

            yield return new RequisicaoAgendamento
            {
                QuantidadePessoas = 10,
                DataInicio = dataInicio, /* 10:00 da manhã, 02 horas reunião*/
                HoraInicio = TimeSpan.FromHours(10),
                DataFim = dataInicio,
                HoraFim = TimeSpan.FromHours(12),
                AcessoInternet = true,
                TvWebCam = false
            };
        }
    }
}
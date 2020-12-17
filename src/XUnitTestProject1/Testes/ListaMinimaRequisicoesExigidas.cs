using System;
using System.Collections.Generic;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Internal;

namespace mtgroup.locacao.Testes
{
    public class ListaMinimaRequisicoesExigidas: XUnitTheoryClassData<RequisicaoSalaReuniao>
    {
        protected override IEnumerable<RequisicaoSalaReuniao> GetEnumerableData()
        {
            // uma data/hora independente do momento em q o teste está sendo executado 
            var dataReferencia = DateTime.Now.Date
                .AddDays(2); /* critério de no mínimo 01 dia de antecedência */
            
            var dataInicio = DateSystemUtils
                .NearestWorkDateBetween(dataReferencia, dataReferencia.AddDays(38));

            var dataRef = DateTime.Now;

            yield return new RequisicaoSalaReuniao(dataRef)
            {
                Periodo = new PeriodoLocacao(dataInicio.AddHours(10), 02), /* 10:00 da manhã, 02 horas reunião*/
                QuantidadePessoas = 10,
                Recursos = RecursoSalaReuniao.AcessoInternet| RecursoSalaReuniao.Televisor| RecursoSalaReuniao.WebCam
            };

            yield return new RequisicaoSalaReuniao(dataRef)
            {
                Periodo = new PeriodoLocacao(dataInicio.AddHours(10), 02), /* 10:00 da manhã, 02 horas reunião*/
                QuantidadePessoas = 10,
                Recursos = RecursoSalaReuniao.AcessoInternet | RecursoSalaReuniao.Televisor | RecursoSalaReuniao.WebCam
            };

            yield return new RequisicaoSalaReuniao(dataRef)
            {
                Periodo = new PeriodoLocacao(dataInicio.AddHours(10), 02), /* 10:00 da manhã, 02 horas reunião*/
                QuantidadePessoas = 10,
                Recursos = RecursoSalaReuniao.AcessoInternet 
            };
        }
    }
}
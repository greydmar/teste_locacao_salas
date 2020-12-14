using FluentResults;

namespace mtgroup.locacao.DataModel
{
    public class ReservaSalaReuniao
    {
        public Solicitante Solicitante { get; }

        public PeriodoLocacao Periodo { get; set; }

        public ushort QuantidadePessoas { get; set; }
    }

    public class ResultadoReservaSala : ResultBase
    {
        public ResultadoReservaSala(ReservaSalaReuniao entrada
            , Reason ocorrencia, bool? ehAceitavel = null) 
        {
        }
    }
}
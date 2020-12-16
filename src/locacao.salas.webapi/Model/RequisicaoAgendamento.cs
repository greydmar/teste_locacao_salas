using System;

namespace mtgroup.locacao.Model
{
    public class RequisicaoAgendamento
    {
        public DateTime DataInicio { get; set; }
        
        public TimeSpan HoraInicio { get; set; }

        public DateTime DataFim { get; set; }

        public TimeSpan HoraFim { get; set; }
        
        public int QuantidadePessoas { get; set; }
        
        public bool AcessoInternet { get; set; }

        public bool TvWebCam { get; set; }
    }

    public class RespostaAgendamento
    {
        public string IdSalaReservada { get; set; }
    }
}
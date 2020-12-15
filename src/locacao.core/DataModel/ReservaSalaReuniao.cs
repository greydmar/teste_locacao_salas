using mtgroup.locacao.Interfaces;

namespace mtgroup.locacao.DataModel
{
    public class ReservaSalaReuniao: IEntidade
    {
        public ReservaSalaReuniao() {}

        public int Id { get; internal set; }

        public string IdSalaReservada { get; set; }

        public Solicitante Solicitante { get; set; }

        public string CodigoReserva { get; set; }

        public PeriodoLocacao Periodo { get; set; }

        public ushort QuantidadePessoas { get; set; }
    }
}
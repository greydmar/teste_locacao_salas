using System;

namespace mtgroup.locacao.DataModel
{
    public class PeriodoLocacao : IEquatable<PeriodoLocacao>
    {
        public PeriodoLocacao(DateTime inicio, TimeSpan duracao)
        {
            this.Inicio = inicio;
            this.Termino = inicio + duracao;
        }

        public PeriodoLocacao(DateTime inicio, ushort horasDuracao)
        {
            this.Inicio = inicio;
            this.Termino = inicio + TimeSpan.FromHours(horasDuracao);
        }

        internal PeriodoLocacao(DateTime inicio, DateTime termino)
        {
            this.Inicio = inicio;
            this.Termino = termino;
        }

        public DateTime Inicio { get; }

        public DateTime Termino { get; }

        public long Horas => Convert.ToInt64((Termino - Inicio).TotalHours);

        public static implicit operator DateTime(PeriodoLocacao periodo)
        {
            return periodo.Inicio.Date;
        }

        public bool Equals(PeriodoLocacao other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Inicio.Equals(other.Inicio);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PeriodoLocacao) obj);
        }

        public override int GetHashCode()
        {
            return Inicio.GetHashCode();
        }

        public static bool operator <(PeriodoLocacao left, TimeSpan right)
        {
            return (left.Termino - left.Inicio) < right;
        }

        public static bool operator >(PeriodoLocacao left, TimeSpan right)
        {
            return (left.Termino - left.Inicio) > right;
        }

        public static bool operator ==(PeriodoLocacao left, PeriodoLocacao right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PeriodoLocacao left, PeriodoLocacao right)
        {
            return !Equals(left, right);
        }

        public bool FinalNoMesmoDia()
        {
            return this.Inicio.Date.Equals(this.Termino.Date);
        }
    }
}
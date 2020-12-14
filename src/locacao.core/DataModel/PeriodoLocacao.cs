using System;

namespace mtgroup.locacao.DataModel
{
    public class PeriodoLocacao : IEquatable<PeriodoLocacao>
    {
        public PeriodoLocacao(in DateTime dataInicio, in TimeSpan duracao)
        {
            this.DataInicio = dataInicio;
            this.Termino = dataInicio + duracao;
        }

        public DateTime DataInicio { get; }

        public DateTime Termino { get; }

        public long Horas => Convert.ToInt64((Termino - DataInicio).TotalHours);

        public static implicit operator DateTime(PeriodoLocacao periodo)
        {
            return periodo.DataInicio.Date;
        }

        public bool Equals(PeriodoLocacao other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DataInicio.Equals(other.DataInicio);
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
            return DataInicio.GetHashCode();
        }

        public static bool operator <(PeriodoLocacao left, TimeSpan right)
        {
            return (left.Termino - left.DataInicio) < right;
        }

        public static bool operator >(PeriodoLocacao left, TimeSpan right)
        {
            return (left.Termino - left.DataInicio) > right;
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
            return this.DataInicio.Date.Equals(this.Termino.Date);
        }
    }
}
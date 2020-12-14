using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ag3.Util.Mensagens
{
    public class ChaveOcorrencia : IChaveOcorrencia
    {
        private readonly IComparable[] _partes;

        public ChaveOcorrencia(params IComparable[] partes)
        {
            this._partes = partes.ToArray();
            this.Length = partes.Length;
        }

        public object this[int index] => _partes[index];

        public int Length { get; }

        public int CompareTo(object? other, IComparer comparer)
        {
            if (other == null)
                throw new ArgumentException(nameof(other));

            if (ReferenceEquals(this, other))
                return 0;

            if (!(other is IChaveOcorrencia chave))
                throw new ArgumentException(nameof(other));
            
            return this.CompararPartes(chave, comparer);
        }

        private int CompararPartes(IChaveOcorrencia chave, IComparer comparer)
        {
            if (this.Length != chave.Length)
                return this.Length - chave.Length;

            return ((IStructuralComparable) this._partes).CompareTo(chave.Partes.ToArray(), comparer);
        }

        private bool CompararPartes(IChaveOcorrencia chave, IEqualityComparer comparer)
        {
            if (this.Length != chave.Length)
                return false;

            return ((IStructuralEquatable) this._partes).Equals(chave.Partes.ToArray(), comparer);
        }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!(other is IChaveOcorrencia chave))
                return false;

            return CompararPartes(chave, comparer);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            var hcode = new HashCode();
            
            for (int i = (this.Length >= 8 ? this.Length - 8 : 0); i < this.Length; i++) {
                hcode.Add(comparer.GetHashCode(_partes[i]));
            }

            return hcode.ToHashCode();
        }

        public IEnumerable<IComparable> Partes => _partes;
    }
}
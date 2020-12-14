using System;
using System.Collections.Generic;

namespace Ag3.Util
{
    public abstract class Comparavel<TValor>: IComparable<Comparavel<TValor>>, IComparable, IEquatable<Comparavel<TValor>>, IEqualityComparer<Comparavel<TValor>>
        where TValor: struct, IComparable<TValor>
    {
        private readonly TValor _code;
        private readonly int _hcode;

        protected Comparavel(TValor code)
        {
            _code = code;
            _hcode = code.GetHashCode();
        }

        public bool Equals(Comparavel<TValor> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _code.Equals(other._code);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Comparavel<TValor>) obj);
        }

        public override int GetHashCode()
        {
            return _hcode;
        }

        public int CompareTo(Comparavel<TValor> other)
        {
            return this._code.CompareTo(other._code);
        }

        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(null, obj)) return -1;
            if (ReferenceEquals(this, obj)) return 0;

            if (obj is Comparavel<TValor> objRecord)
                return this.CompareTo(objRecord);
            if (obj is IComparable<TValor> objComparable)
                return objComparable.CompareTo(this._code);
            if (obj is IComparable untypedComparable)
                return untypedComparable.CompareTo(this._code);

            throw new NotSupportedException("Unknown comparison!");
        }

        public static bool operator ==(Comparavel<TValor> left, Comparavel<TValor> right)
        {
            return object.Equals(left, right);
        }

        public static bool operator !=(Comparavel<TValor> left, Comparavel<TValor> right)
        {
            return !object.Equals(left, right);
        }

        public bool Equals(Comparavel<TValor> x, Comparavel<TValor> y)
        {
            return object.Equals(x, y);
        }

        public int GetHashCode(Comparavel<TValor> obj)
        {
            return obj?.GetHashCode() ?? 0;
        }
    }
}
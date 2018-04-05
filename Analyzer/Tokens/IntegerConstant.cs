using System;

namespace Analyzer.Tokens
{
    public class IntegerConstant : Token, IEquatable<IntegerConstant>
    {
        public int Integer { get; }

        public IntegerConstant(int integer) : base(integer.ToString())
        {
            Integer = integer;
        }

        public bool Equals(IntegerConstant other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Integer == other.Integer;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IntegerConstant) obj);
        }

        public override int GetHashCode() => Integer;
        public static bool operator ==(IntegerConstant left, IntegerConstant right) => Equals(left, right);
        public static bool operator !=(IntegerConstant left, IntegerConstant right) => !Equals(left, right);
        public override string ToString() => Integer.ToString();
    }
}
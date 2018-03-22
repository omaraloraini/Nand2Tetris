using System;

namespace Analyzer
{
    public class IntegerToken : Token, IEquatable<IntegerToken>
    {
        public int Integer { get; }

        public IntegerToken(int integer)
        {
            Integer = integer;
        }

        public bool Equals(IntegerToken other)
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
            return Equals((IntegerToken) obj);
        }

        public override int GetHashCode() => Integer;
        public static bool operator ==(IntegerToken left, IntegerToken right) => Equals(left, right);
        public static bool operator !=(IntegerToken left, IntegerToken right) => !Equals(left, right);
        public override string ToString() => Integer.ToString();
    }
}
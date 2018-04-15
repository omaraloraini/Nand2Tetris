using System;
using Analyzer.Expressions;

namespace Analyzer.Tokens
{
    public class StringConstant : Token, ITerm, IEquatable<StringConstant>
    {
        public StringConstant(string @string) : base(@string)
        {
        }

        public bool Equals(StringConstant other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StringConstant) obj);
        }

        public override int GetHashCode() => (Value != null ? Value.GetHashCode() : 0);
        public static bool operator ==(StringConstant left, StringConstant right) => Equals(left, right);
        public static bool operator !=(StringConstant left, StringConstant right) => !Equals(left, right);
        public override string ToString() => Value;
    }
}
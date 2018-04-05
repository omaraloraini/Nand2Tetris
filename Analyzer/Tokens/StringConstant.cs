using System;

namespace Analyzer.Tokens
{
    public class StringConstant : Token, IEquatable<StringConstant>
    {
        public string String { get; }

        public StringConstant(string @string) : base(@string)
        {
            String = @string;
        }

        public bool Equals(StringConstant other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(String, other.String);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StringConstant) obj);
        }

        public override int GetHashCode() => (String != null ? String.GetHashCode() : 0);
        public static bool operator ==(StringConstant left, StringConstant right) => Equals(left, right);
        public static bool operator !=(StringConstant left, StringConstant right) => !Equals(left, right);
        public override string ToString() => String;
    }
}
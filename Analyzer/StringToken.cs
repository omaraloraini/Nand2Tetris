using System;

namespace Analyzer
{
    public class StringToken : Token, IEquatable<StringToken>
    {
        public string String { get; }

        public StringToken(string @string)
        {
            String = @string;
        }

        public bool Equals(StringToken other)
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
            return Equals((StringToken) obj);
        }

        public override int GetHashCode() => (String != null ? String.GetHashCode() : 0);
        public static bool operator ==(StringToken left, StringToken right) => Equals(left, right);
        public static bool operator !=(StringToken left, StringToken right) => !Equals(left, right);
        public override string ToString() => String;
    }
}
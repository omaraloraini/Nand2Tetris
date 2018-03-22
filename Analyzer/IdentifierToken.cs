using System;

namespace Analyzer
{
    public class IdentifierToken : Token, IEquatable<IdentifierToken>
    {
        public string Identifier { get; }

        public IdentifierToken(string identifier)
        {
            Identifier = identifier;
        }

        public bool Equals(IdentifierToken other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Identifier, other.Identifier);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IdentifierToken) obj);
        }

        public override int GetHashCode() => Identifier.GetHashCode();
        public static bool operator ==(IdentifierToken left, IdentifierToken right) => Equals(left, right);
        public static bool operator !=(IdentifierToken left, IdentifierToken right) => !Equals(left, right);
        public override string ToString() => Identifier;
    }
}
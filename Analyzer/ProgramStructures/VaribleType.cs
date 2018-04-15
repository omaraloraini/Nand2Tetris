using System;
using Analyzer.Tokens;

namespace Analyzer.ProgramStructures
{
    public class VaribleType : IEquatable<VaribleType>
    {
        public bool Equals(VaribleType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Token, other.Token);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VaribleType) obj);
        }
        public override int GetHashCode() => (Token != null ? Token.GetHashCode() : 0);
        public static bool operator ==(VaribleType left, VaribleType right) => Equals(left, right);
        public static bool operator !=(VaribleType left, VaribleType right) => !Equals(left, right);

        public Token Token { get; }

        public bool IsClass() => Token is Identifier;

        public VaribleType(Token token)
        {
            if (!IsValid(token)) throw new ArgumentException("Invalid type");
            Token = token;
        }
            
        public static bool IsValid(Token token)
        {
            return
                token is Identifier ||
                token is Keyword k && (
                    k == Keyword.Int || k == Keyword.Char || k == Keyword.Boolean);
        }
    }
}
using System;
using Analyzer.Tokens;

namespace Analyzer.ProgramStructures
{
    public class VaribleType : IEquatable<VaribleType>
    {
        private readonly Token _token;
        public string Name => _token.Value;

        public bool IsClass() => _token is Identifier;

        public VaribleType(Token token)
        {
            if (!IsValid(token)) throw new ArgumentException("Invalid type");
            _token = token;
        }

        public VaribleType(string type) : this(new Identifier(type)) {}
            
        public static bool IsValid(Token token)
        {
            return
                token is Identifier ||
                token is Keyword k && (
                    k == Keyword.Int || k == Keyword.Char ||
                    k == Keyword.Boolean || k == Keyword.Void);
        }

        public bool Equals(VaribleType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_token, other._token);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VaribleType) obj);
        }

        public override int GetHashCode()
        {
            return (_token != null ? _token.GetHashCode() : 0);
        }

        public static bool operator ==(VaribleType left, VaribleType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VaribleType left, VaribleType right)
        {
            return !Equals(left, right);
        }
    }
}
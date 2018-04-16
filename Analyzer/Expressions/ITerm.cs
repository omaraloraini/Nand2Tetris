using System;
using Analyzer.Tokens;

namespace Analyzer.Expressions
{    
    public interface ITerm {}
    
    public interface IVarible : ITerm {}

    public class ArrayIdentifier : IVarible
    {
        public Identifier Identifier { get; }
        public Expression Expression { get; }

        public ArrayIdentifier(Identifier identifier, Tokenizer tokenizer)
        {
            Identifier = identifier;

            tokenizer.CurrentIs(Symbol.OpenBracket).Move();
            Expression = new Expression(tokenizer);
            tokenizer.CurrentIs(Symbol.CloseBracket).Move();
        }
    }
    
    public class UnaryTerm : ITerm, IEquatable<UnaryTerm>
    {
        public bool Equals(UnaryTerm other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Symbol, other.Symbol) && Equals(Term, other.Term);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UnaryTerm) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Symbol != null ? Symbol.GetHashCode() : 0) * 397) ^ (Term != null ? Term.GetHashCode() : 0);
            }
        }

        public static bool operator ==(UnaryTerm left, UnaryTerm right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UnaryTerm left, UnaryTerm right)
        {
            return !Equals(left, right);
        }

        public Symbol Symbol { get; }
        public ITerm Term { get; }

        public UnaryTerm(Symbol symbol, ITerm term)
        {
            if (symbol != Symbol.Minus && symbol != Symbol.Tilde)
                throw new ArgumentException();

            Symbol = symbol;
            Term = term;
        }
    }


}
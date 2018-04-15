using System;
using System.Text.RegularExpressions;

namespace Analyzer.Tokens
{
    public class Symbol : Token, IEquatable<Symbol>
    {
        private static Regex _symbolRegex = new Regex(
            @"{|}|\(|\)|\[|\]|\.|,|;|\+|\-|\*|/|&|\||<|>|=|~");
        
        public Symbol(char value) : base(value.ToString())
        {
            if (!_symbolRegex.IsMatch(value.ToString()))
                throw new ArgumentException($"{value} : invalid symbol");    
        }
        
        public static bool IsSymbol(char symbol) =>
            _symbolRegex.IsMatch(symbol.ToString());
        
        public static Symbol OpenCurly => new Symbol('{');
        public static Symbol CloseCurly => new Symbol('}');
        public static Symbol OpenParenthesis => new Symbol('(');
        public static Symbol CloseParenthesis => new Symbol(')');
        public static Symbol OpenBracket => new Symbol('[');
        public static Symbol CloseBracket => new Symbol(']');
        public static Symbol Dot => new Symbol('.');
        public static Symbol Commna => new Symbol(',');
        public static Symbol SemiColon => new Symbol(';');
        public static Symbol Plus => new Symbol('+');
        public static Symbol Minus => new Symbol('-');
        public static Symbol Star => new Symbol('*');
        public static Symbol Slash => new Symbol('/');
        public static Symbol Ampersand => new Symbol('&');
        public static Symbol Pipe => new Symbol('|');
        public static Symbol LessThan => new Symbol('<');
        public static Symbol GreaterThan => new Symbol('>');
        public static Symbol Equal => new Symbol('=');
        public static Symbol Tilde => new Symbol('~');

        public bool Equals(Symbol other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Symbol) obj);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(Symbol left, Symbol right) => Equals(left, right);
        public static bool operator !=(Symbol left, Symbol right) => !Equals(left, right);
        public override string ToString() => Value;
    }
}
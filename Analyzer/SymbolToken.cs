using System;
using System.Text.RegularExpressions;

namespace Analyzer
{
    public class SymbolToken : Token, IEquatable<SymbolToken>
    {
        public char Symbol { get; }

        private static Regex _symbolRegex = new Regex(
            @"{|}|\(|\)|\[|\]|\.|,|;|\+|\-|\*|/|&|\||<|>|=|~");
        
        public SymbolToken(char symbol)
        {
            if (!_symbolRegex.IsMatch(symbol.ToString()))
                throw new ArgumentException($"{symbol} : invalid symbol");
            
            Symbol = symbol;
        }
        
        public static bool IsSymbol(char symbol) =>
            _symbolRegex.IsMatch(symbol.ToString());
        
        public static SymbolToken OpenCurly => new SymbolToken('{');
        public static SymbolToken CloseCurly => new SymbolToken('}');
        public static SymbolToken OpenParenthesis => new SymbolToken('(');
        public static SymbolToken CloseParenthesis => new SymbolToken(')');
        public static SymbolToken OpenBracket => new SymbolToken('[');
        public static SymbolToken CloseBracket => new SymbolToken(']');
        public static SymbolToken Dot => new SymbolToken('.');
        public static SymbolToken Commna => new SymbolToken(',');
        public static SymbolToken SemiColon => new SymbolToken(';');
        public static SymbolToken Plus => new SymbolToken('+');
        public static SymbolToken Minus => new SymbolToken('-');
        public static SymbolToken Star => new SymbolToken('*');
        public static SymbolToken Slash => new SymbolToken('/');
        public static SymbolToken Ampersand => new SymbolToken('&');
        public static SymbolToken Pipe => new SymbolToken('|');
        public static SymbolToken LessThan => new SymbolToken('<');
        public static SymbolToken GreaterThan => new SymbolToken('>');
        public static SymbolToken Equal => new SymbolToken('=');
        public static SymbolToken Tilde => new SymbolToken('~');

        public bool Equals(SymbolToken other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Symbol == other.Symbol;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SymbolToken) obj);
        }

        public override int GetHashCode() => Symbol.GetHashCode();
        public static bool operator ==(SymbolToken left, SymbolToken right) => Equals(left, right);
        public static bool operator !=(SymbolToken left, SymbolToken right) => !Equals(left, right);
        public override string ToString() => Symbol.ToString();
    }
}
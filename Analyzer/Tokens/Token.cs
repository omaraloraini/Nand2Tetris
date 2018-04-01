using System;

namespace Analyzer.Tokens
{
    public abstract class Token : IToken
    {
        public static Token Parse(string value)
        {
            if (int.TryParse(value, out var i)) return new IntegerToken(i);

            if (value.StartsWith('"') && value.EndsWith('"'))
                return new StringToken(value.Substring(1, value.Length - 2));
            
            if (KeywordToken.IsKeyword(value))
                return new KeywordToken(value);
            
            if (value.Length == 1 && SymbolToken.IsSymbol(value[0]))
                return new SymbolToken(value[0]);
            
            if (!value.Contains(" ")) 
                return new IdentifierToken(value);
            
            throw new ArgumentException($"{value} : invalid token");
        }

        public bool IsOperator()
        {
            return this is SymbolToken symbolToken &&(
                   symbolToken == SymbolToken.Minus ||
                   symbolToken == SymbolToken.Star ||
                   symbolToken == SymbolToken.Slash ||
                   symbolToken == SymbolToken.Ampersand ||
                   symbolToken == SymbolToken.Pipe ||
                   symbolToken == SymbolToken.LessThan ||
                   symbolToken == SymbolToken.GreaterThan ||
                   symbolToken == SymbolToken.Equal);
        }

        public bool IsUnaryOperator()
        {
            return this is SymbolToken symbolToken && (
                       symbolToken == SymbolToken.Tilde ||
                       symbolToken == SymbolToken.Minus);
        }
    }
}
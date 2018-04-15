using System;

namespace Analyzer.Tokens
{
    public abstract class Token
    {
        public string Value { get; }

        protected Token(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        public static Token Parse(string value)
        {
            if (int.TryParse(value, out var i)) return new IntegerConstant(i);

            if (value.StartsWith('"') && value.EndsWith('"'))
                return new StringConstant(value.Substring(1, value.Length - 2));

            // TODO: FACTORY METHOD TO AVOID CREATING NEW OBJECTS
            if (KeywordConstant.IsKeywordConstant(value))
                return new KeywordConstant(value);
                
            if (Keyword.IsKeyword(value))
                return new Keyword(value);
            
            if (value.Length == 1 && Symbol.IsSymbol(value[0]))
                return new Symbol(value[0]);
            
            if (!value.Contains(" ")) 
                return new Identifier(value);
            
            throw new ArgumentException($"{value} : invalid token");
        }

        public bool IsOperator()
        {
            return this is Symbol symbolToken &&(
                   symbolToken == Symbol.Plus ||
                   symbolToken == Symbol.Minus ||
                   symbolToken == Symbol.Star ||
                   symbolToken == Symbol.Slash ||
                   symbolToken == Symbol.Ampersand ||
                   symbolToken == Symbol.Pipe ||
                   symbolToken == Symbol.LessThan ||
                   symbolToken == Symbol.GreaterThan ||
                   symbolToken == Symbol.Equal);
        }

        public bool IsUnaryOperator()
        {
            return this is Symbol symbolToken && (
                       symbolToken == Symbol.Tilde ||
                       symbolToken == Symbol.Minus);
        }
    }
}
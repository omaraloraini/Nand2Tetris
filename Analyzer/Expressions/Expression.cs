using System;
using System.Collections.Generic;
using Analyzer.Tokens;

namespace Analyzer.Expressions
{
    public static class Terms
    {
        public static ITerm Parse(Tokenizer tokenizer)
        {
            switch (tokenizer.GetCurrentThenMove())
            {
                case IntegerConstant integerConstant: return integerConstant;
                case StringConstant stringConstant: return stringConstant;
                case KeywordConstant keyword:return keyword;
                case Symbol symbolToken when symbolToken == Symbol.OpenParenthesis:
                    var expression = new Expression(tokenizer);
                    tokenizer.CurrentIs(Symbol.CloseParenthesis).Move();
                    return expression;
                case Symbol symbolToken 
                when symbolToken == Symbol.Minus || symbolToken == Symbol.Tilde:
                    return new UnaryTerm(symbolToken, Parse(tokenizer));
                case Identifier identifier:
                    if (tokenizer.Current != null && tokenizer.Current.Equals(Symbol.OpenBracket))
                    {
                        return new ArrayIdentifier(identifier, tokenizer);
                    }
                    else if (tokenizer.Current != null && tokenizer.Current.Equals(Symbol.OpenParenthesis))
                    {
                        return new SubroutineCall(null, identifier, tokenizer);
                    }
                    else if (tokenizer.Current != null && tokenizer.Current.Equals(Symbol.Dot))
                    {
                        var subrotineName = tokenizer
                            .Move()
                            .CurrentIsIdentifier()
                            .Current as Identifier;

                        return new SubroutineCall(identifier, subrotineName, tokenizer.Move());
                    }
                    else
                    {
                        return identifier;
                    }
            }

            throw new Exception();
        }
    }
    
    public class Expression : ITerm
    {
        public ITerm Term { get; }
        public IList<(Symbol, ITerm)> OperatorTermPairs { get; } = new List<(Symbol, ITerm)>();

        public Expression(Tokenizer tokenizer)
        {
            Term = Terms.Parse(tokenizer);

            while (tokenizer.Current != null &&
                tokenizer.Current is Symbol symbol && tokenizer.Current.IsOperator())
            {
                OperatorTermPairs.Add((symbol, Terms.Parse(tokenizer.Move())));
            }
        }
    }
}
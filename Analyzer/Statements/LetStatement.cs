using System;
using Analyzer.Expressions;
using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class LetStatement : Statement
    {
        public LetStatement(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(Keyword.Let)
                .ApplyThenMove(AddCurrent)
                .CurrentIsIdentifier()
                .ApplyThenMove(AddCurrent)
                .ApplyIf(Symbol.OpenBracket, t => t
                    .Apply(ExpressionWithInBrackets))
                .CurrentIs(Symbol.Equal)
                .ApplyThenMove(AddCurrent)
                .Apply(AddExpresion)
                .CurrentIs(Symbol.SemiColon)
                .ApplyThenMove(AddCurrent);
        }
    }
}
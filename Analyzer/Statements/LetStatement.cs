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
                .CurrentIs(KeywordToken.Let)
                .ApplyThenMove(AddCurrent)
                .CurrentIsIdentifier()
                .ApplyThenMove(AddCurrent)
                .ApplyIf(SymbolToken.OpenBracket, t => t
                    .Apply(ExpressionWithInBrackets))
                .CurrentIs(SymbolToken.Equal)
                .Apply(AddExpresion)
                .CurrentIs(SymbolToken.SemiColon)
                .ApplyThenMove(AddCurrent);
        }
    }
}
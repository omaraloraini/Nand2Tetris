using Analyzer.Tokens;

namespace Analyzer.Expressions
{
    public class SubroutineCall : Expression
    {
        public SubroutineCall(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIsIdentifier()
                .ApplyThenMove(AddCurrent)
                .ApplyIf(SymbolToken.Dot, t => t
                    .ApplyThenMove(AddCurrent)
                    .CurrentIsIdentifier()
                    .ApplyThenMove(AddCurrent))
                .CurrentIs(SymbolToken.OpenParenthesis)
                .ApplyThenMove(AddCurrent)
                .Apply(ExpressionList)
                .CurrentIs(SymbolToken.CloseParenthesis)
                .ApplyThenMove(AddCurrent);
        }
    }
}
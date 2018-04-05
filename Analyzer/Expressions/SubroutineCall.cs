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
                .ApplyIf(Symbol.Dot, t => t
                    .ApplyThenMove(AddCurrent)
                    .CurrentIsIdentifier()
                    .ApplyThenMove(AddCurrent))
                .CurrentIs(Symbol.OpenParenthesis)
                .ApplyThenMove(AddCurrent)
                .Apply(ExpressionList)
                .CurrentIs(Symbol.CloseParenthesis)
                .ApplyThenMove(AddCurrent);
        }
    }
}
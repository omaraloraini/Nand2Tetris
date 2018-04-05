using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class ReturntStatment : Statement
    {
        public ReturntStatment(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(Keyword.Return)
                .ApplyThenMove(AddCurrent)
                .ApplyIfNot(Symbol.SemiColon, t => t
                    .Apply(AddExpresion))
                .CurrentIs(Symbol.SemiColon)
                .ApplyThenMove(AddCurrent);
        }
    }
}
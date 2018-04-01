using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class ReturntStatment : Statement
    {
        public ReturntStatment(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(KeywordToken.Return)
                .ApplyThenMove(AddCurrent)
                .ApplyIfNot(SymbolToken.SemiColon, t => t
                    .Apply(AddExpresion))
                .CurrentIs(SymbolToken.SemiColon)
                .ApplyThenMove(AddCurrent);
        }
    }
}
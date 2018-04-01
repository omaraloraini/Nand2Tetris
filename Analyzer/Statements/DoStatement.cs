using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class DoStatement : Statement
    {
        public DoStatement(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(KeywordToken.Do)
                .ApplyThenMove(AddCurrent)
                .Apply(SubroutineCall)
                .CurrentIs(SymbolToken.SemiColon)
                .ApplyThenMove(AddCurrent);
        }
    }
}
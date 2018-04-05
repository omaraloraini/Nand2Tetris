using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class DoStatement : Statement
    {
        public DoStatement(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(Keyword.Do)
                .ApplyThenMove(AddCurrent)
                .Apply(SubroutineCall)
                .CurrentIs(Symbol.SemiColon)
                .ApplyThenMove(AddCurrent);
        }
    }
}
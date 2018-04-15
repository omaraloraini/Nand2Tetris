using Analyzer.Expressions;
using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class DoStatement : Statement
    {
        public SubroutineCall SubroutineCall { get; }
        public DoStatement(Tokenizer tokenizer)
        {
            tokenizer.CurrentIs(Keyword.Do).Move();
            SubroutineCall = new SubroutineCall(null,
                tokenizer.GetCurrentThenMove() as Identifier, tokenizer);

            tokenizer.CurrentIs(Symbol.SemiColon).Move();
        }
    }
}
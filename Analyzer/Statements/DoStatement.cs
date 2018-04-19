using Analyzer.Expressions;
using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class DoStatement : Statement
    {
        public SubroutineCall SubroutineCall { get; }
        public DoStatement(Tokenizer tokenizer)
        {
            tokenizer.CurrentIs(Keyword.Do).Move().CurrentIsIdentifier();

            SubroutineCall = Terms.Parse(tokenizer) as SubroutineCall;
            
            tokenizer.CurrentIs(Symbol.SemiColon).Move();
        }
    }
}
using Analyzer.Expressions;
using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class ReturntStatment : Statement
    {
        public Expression Expression { get;}
        public ReturntStatment(Tokenizer tokenizer)
        {
            tokenizer.CurrentIs(Keyword.Return).Move();

            if (!tokenizer.Current.Equals(Symbol.SemiColon))
            {
                Expression = new Expression(tokenizer);
            }

            tokenizer.CurrentIs(Symbol.SemiColon).Move();
        }
    }
}
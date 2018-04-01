using Analyzer.Tokens;

namespace Analyzer.Expressions
{
    public class Expression : CompositeToken
    {
        protected Expression(){}
        public Expression(Tokenizer tokenizer)
        {
            do
            {
                Tokens.Add(new Term(tokenizer));
                tokenizer.ApplyThenMove(AddCurrent);
            } while (!tokenizer.Current.IsOperator());
        }
    }
}
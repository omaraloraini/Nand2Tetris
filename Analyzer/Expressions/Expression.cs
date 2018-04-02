using Analyzer.Tokens;

namespace Analyzer.Expressions
{
    public class Expression : CompositeToken
    {
        protected Expression(){}
        public Expression(Tokenizer tokenizer)
        {
            Tokens.Add(new Term(tokenizer));
            while (tokenizer.Current.IsOperator())
            {
                tokenizer.ApplyThenMove(AddCurrent);
                Tokens.Add(new Term(tokenizer));
            }
        }
    }
}
using Analyzer.Tokens;

namespace Analyzer.Expressions
{
    public class ExpressionList : Expression
    {
        public ExpressionList(Tokenizer tokenizer)
        {
            while (!tokenizer.Current.Equals(Symbol.CloseParenthesis))
            {
                if (tokenizer.Current.Equals(Symbol.Commna))
                {
                    tokenizer.ApplyThenMove(AddCurrent);
                }

                tokenizer.Apply(AddExpresion);
            }
        }
    }
}
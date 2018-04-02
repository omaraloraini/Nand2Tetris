using Analyzer.Tokens;

namespace Analyzer.Expressions
{
    public class ExpressionList : Expression
    {
        public ExpressionList(Tokenizer tokenizer)
        {
            while (!tokenizer.Current.Equals(SymbolToken.CloseParenthesis))
            {
                if (tokenizer.Current.Equals(SymbolToken.Commna))
                {
                    tokenizer.ApplyThenMove(AddCurrent);
                }

                tokenizer.Apply(AddExpresion);
            }
        }
    }
}
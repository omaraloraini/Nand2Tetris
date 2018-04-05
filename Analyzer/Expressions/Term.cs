using Analyzer.Tokens;

namespace Analyzer.Expressions
{
    public class Term : Expression
    {
        public Term(Tokenizer tokenizer)
        {
            tokenizer
                .ApplyIf(Symbol.Minus, t => t.ApplyThenMove(AddCurrent))
                .ApplyIf(Symbol.Tilde, t => t.ApplyThenMove(AddCurrent));

            switch (tokenizer.Current)
            {
                case Symbol symbolToken when symbolToken == Symbol.OpenParenthesis:
                    tokenizer
                        .ApplyThenMove(AddCurrent)
                        .Apply(AddExpresion)
                        .CurrentIs(Symbol.CloseParenthesis)
                        .ApplyThenMove(AddCurrent);
                    break;
                case IntegerConstant _:
                case StringConstant _:
                case Keyword _:
                    tokenizer.ApplyThenMove(AddCurrent);
                    break;
                case Identifier _:
                    if (tokenizer.Next.Equals(Symbol.OpenBracket))
                    {
                        tokenizer
                            .ApplyThenMove(AddCurrent)
                            .Apply(ExpressionWithInBrackets);
                    }
                    else if (tokenizer.Next.Equals(Symbol.Dot) || 
                             tokenizer.Next.Equals(Symbol.OpenParenthesis))
                    {
                        tokenizer.Apply(SubroutineCall);
                    }
                    else
                    {
                        tokenizer.ApplyThenMove(AddCurrent);
                    }
                    break;
            }
        }
    }
}
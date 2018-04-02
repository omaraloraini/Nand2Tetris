using Analyzer.Tokens;

namespace Analyzer.Expressions
{
    public class Term : Expression
    {
        public Term(Tokenizer tokenizer)
        {
            tokenizer
                .ApplyIf(SymbolToken.Minus, t => t.ApplyThenMove(AddCurrent))
                .ApplyIf(SymbolToken.Tilde, t => t.ApplyThenMove(AddCurrent));

            switch (tokenizer.Current)
            {
                case SymbolToken symbolToken when symbolToken == SymbolToken.OpenParenthesis:
                    tokenizer
                        .ApplyThenMove(AddCurrent)
                        .Apply(AddExpresion)
                        .CurrentIs(SymbolToken.CloseParenthesis)
                        .ApplyThenMove(AddCurrent);
                    break;
                case IntegerToken _:
                case StringToken _:
                case KeywordToken _:
                    tokenizer.ApplyThenMove(AddCurrent);
                    break;
                case IdentifierToken _:
                    if (tokenizer.Next.Equals(SymbolToken.OpenBracket))
                    {
                        tokenizer
                            .ApplyThenMove(AddCurrent)
                            .Apply(ExpressionWithInBrackets);
                    }
                    else if (tokenizer.Next.Equals(SymbolToken.Dot) || 
                             tokenizer.Next.Equals(SymbolToken.OpenParenthesis))
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
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
                    tokenizer
                        .ApplyThenMove(AddCurrent)
                        .ApplyIf(SymbolToken.OpenBracket, t => t
                            .Apply(ExpressionWithInBrackets))
                        .ApplyIf(SymbolToken.OpenParenthesis, t => t
                            .Apply(SubroutineCall));
                    break;
            }
        }
    }
}
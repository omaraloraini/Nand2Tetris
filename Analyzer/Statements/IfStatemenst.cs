using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class IfStatemenst : Statement
    {
        public IfStatemenst(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(Keyword.If)
                .ApplyThenMove(AddCurrent)
                .Apply(ExpressionWithInParentheses)
                .Apply(StatementsWithInCurlyBrackets)
                .ApplyIf(Keyword.Else, t => t
                    .ApplyThenMove(AddCurrent)
                    .Apply(StatementsWithInCurlyBrackets));
        }
    }
}
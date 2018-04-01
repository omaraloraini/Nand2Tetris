using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class IfStatemenst : Statement
    {
        public IfStatemenst(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(KeywordToken.If)
                .Apply(AddCurrent)
                .Apply(ExpressionWithInParentheses)
                .Apply(StatementsWithInCurlyBrackets)
                .ApplyIf(KeywordToken.Else, t => t
                    .ApplyThenMove(AddCurrent)
                    .Apply(StatementsWithInCurlyBrackets));
        }
    }
}
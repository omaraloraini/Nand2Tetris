using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class WhileStatement : Statement
    {
        public WhileStatement(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(Keyword.While)
                .ApplyThenMove(AddCurrent)
                .Apply(ExpressionWithInParentheses)
                .Apply(StatementsWithInCurlyBrackets);
        }
    }
}
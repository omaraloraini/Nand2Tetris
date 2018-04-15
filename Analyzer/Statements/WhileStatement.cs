using System.Collections.Generic;
using Analyzer.Expressions;
using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class WhileStatement : Statement
    {
        public Expression Condition { get; set; }
        public IEnumerable<Statement> Statements { get; }
        public WhileStatement(Tokenizer tokenizer)
        {
            tokenizer.CurrentIs(Keyword.While).Move().CurrentIs(Symbol.OpenParenthesis);

            Condition = Terms.Parse(tokenizer) as Expression;

            tokenizer.CurrentIs(Symbol.OpenCurly).Move();
            Statements = ParseStatements(tokenizer);
            tokenizer.CurrentIs(Symbol.CloseCurly).Move();
        }

    }
}
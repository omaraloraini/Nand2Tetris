using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Analyzer.Expressions;
using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class IfStatement : Statement
    {
        public Expression Condition { get; }
        public IEnumerable<Statement> TrueBranch { get; }
        public IEnumerable<Statement> FalseBranch { get; } = ImmutableList<Statement>.Empty;
        
        public IfStatement(Tokenizer tokenizer)
        {
            tokenizer.CurrentIs(Keyword.If)
                .Move().CurrentIs(Symbol.OpenParenthesis).Move();
            
            Condition = new Expression(tokenizer);
            tokenizer.CurrentIs(Symbol.CloseParenthesis).Move();

            tokenizer.CurrentIs(Symbol.OpenCurly).Move();
            TrueBranch = ParseStatements(tokenizer).ToList();
            tokenizer.CurrentIs(Symbol.CloseCurly).Move();
            
            if (tokenizer.Current != null && tokenizer.Current.Equals(Keyword.Else))
            {
                tokenizer.Move().CurrentIs(Symbol.OpenCurly).Move();
                FalseBranch = ParseStatements(tokenizer).ToList();
                tokenizer.CurrentIs(Symbol.CloseCurly).Move();
            }
        }
    }
}
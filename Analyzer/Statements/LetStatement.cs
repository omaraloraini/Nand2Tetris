using System;
using Analyzer.Expressions;
using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class LetStatement : Statement
    {
        public IVarible Varible { get; set; }
        public Expression Expression { get; set; }
        
        public LetStatement(Tokenizer tokenizer)
        {
            tokenizer.CurrentIs(Keyword.Let).Move().CurrentIsIdentifier();

            if (tokenizer.Next.Equals(Symbol.OpenBracket))
            {
                Varible = new ArrayIdentifier(tokenizer.GetCurrentThenMove() as Identifier, tokenizer);
            }
            else
            {
                Varible = tokenizer.GetCurrentThenMove() as Identifier;
            }

            tokenizer.CurrentIs(Symbol.Equal).Move();
            
            Expression = new Expression(tokenizer);

            tokenizer.CurrentIs(Symbol.SemiColon).Move();
        }
    }
}
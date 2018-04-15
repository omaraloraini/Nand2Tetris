using System;
using Analyzer.Expressions;
using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class LetStatement : Statement
    {
        public ITerm Varible { get; set; }
        public Expression Expression { get; set; }
        
        public LetStatement(Tokenizer tokenizer)
        {
            tokenizer.CurrentIs(Keyword.Let).Move().CurrentIsIdentifier();

            if (tokenizer.Next.Equals(Symbol.Dot))
            {
                Varible = new ArrayIdentifier(tokenizer.Current as Identifier, tokenizer.Move());
            }
            else
            {
                Varible = tokenizer.Current as Identifier;
            }

            tokenizer.CurrentIs(Symbol.Equal).Move();
            
            Expression = new Expression(tokenizer);

            tokenizer.CurrentIs(Symbol.SemiColon).Move();
        }
    }
}
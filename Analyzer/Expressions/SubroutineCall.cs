using System.Collections.Generic;
using Analyzer.Tokens;

namespace Analyzer.Expressions
{
    public class SubroutineCall : ITerm
    {
        public Identifier CalledOn { get; }
        public Identifier SobroutineName { get; }
        public List<Expression> Parametars { get; } = new List<Expression>();

        public SubroutineCall(Identifier calledOn, Identifier sobroutineName,
            Tokenizer tokenizer)
        {
            tokenizer.CurrentIs(Symbol.OpenParenthesis).Move();
            
            CalledOn = calledOn;
            SobroutineName = sobroutineName;
            while (!tokenizer.Current.Equals(Symbol.CloseParenthesis))
            {
                if (tokenizer.Current.Equals(Symbol.Commna)) tokenizer.Move();

                Parametars.Add(new Expression(tokenizer));
            }
            
            tokenizer.CurrentIs(Symbol.CloseParenthesis).Move();
        }
    }
}
using System.Collections.Generic;
using Analyzer.Expressions;
using Analyzer.Statements;

namespace Analyzer.Tokens
{
    public abstract class CompositeToken : IToken
    {
        public List<IToken> Tokens { get; } = new List<IToken>();
        
        protected void AddCurrent(Tokenizer tokenizer)
        {
            Tokens.Add(tokenizer.Current);
        }

        protected void AddExpresion(Tokenizer tokenizer)
        {
            Tokens.Add(new Expression(tokenizer));
        }

        protected void SubroutineCall(Tokenizer tokenizer)
        {
            Tokens.AddRange(new SubroutineCall(tokenizer).Tokens);
        }

        protected void ExpressionList(Tokenizer tokenizer)
        {
            Tokens.Add(new ExpressionList(tokenizer));
        }

        protected void ExpressionWithInParentheses(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(Symbol.OpenParenthesis)
                .ApplyThenMove(AddCurrent)
                .Apply(AddExpresion)
                .CurrentIs(Symbol.CloseParenthesis)
                .ApplyThenMove(AddCurrent);
        }

        protected void ExpressionWithInBrackets(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(Symbol.OpenBracket)
                .ApplyThenMove(AddCurrent)
                .Apply(AddExpresion)
                .CurrentIs(Symbol.CloseBracket)
                .ApplyThenMove(AddCurrent);
        }
        
        protected void AddStatements(Tokenizer tokenizer)
        {
            Tokens.Add(new Statements.Statements(tokenizer));
        }

        protected void StatementsWithInCurlyBrackets(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(Symbol.OpenCurly)
                .ApplyThenMove(AddCurrent)
                .Apply(AddStatements)
                .CurrentIs(Symbol.CloseCurly)
                .ApplyThenMove(AddCurrent);
        }

    }
}
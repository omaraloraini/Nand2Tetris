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
                .CurrentIs(SymbolToken.OpenParenthesis)
                .ApplyThenMove(AddCurrent)
                .Apply(AddExpresion)
                .CurrentIs(SymbolToken.CloseParenthesis)
                .ApplyThenMove(AddCurrent);
        }

        protected void ExpressionWithInBrackets(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(SymbolToken.OpenBracket)
                .ApplyThenMove(AddCurrent)
                .Apply(AddExpresion)
                .CurrentIs(SymbolToken.CloseBracket)
                .ApplyThenMove(AddCurrent);
        }
        
        protected void AddStatements(Tokenizer tokenizer)
        {
            Tokens.AddRange(Statement.ParseStatements(tokenizer));
        }

        protected void StatementsWithInCurlyBrackets(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(SymbolToken.OpenCurly)
                .ApplyThenMove(AddCurrent)
                .Apply(AddStatements)
                .CurrentIs(SymbolToken.CloseCurly)
                .ApplyThenMove(AddCurrent);
        }

    }
}
using System;
using System.Runtime.InteropServices.ComTypes;
using Analyzer.Tokens;
using static Analyzer.Tokens.Keyword;

namespace Analyzer
{
    public class JackProgram : CompositeToken
    {
        public JackProgram(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(Class)
                .ApplyThenMove(AddCurrent)
                .CurrentIsIdentifier()
                .ApplyThenMove(AddCurrent)
                .CurrentIs(Symbol.OpenCurly)
                .ApplyThenMove(AddCurrent)
                .Apply(ClassVariableDeclarations)
                .Apply(SubroutineDeclarations)
                .CurrentIs(Symbol.CloseCurly)
                .ApplyThenMove(AddCurrent);
        }

        private void SubroutineDeclarations(Tokenizer tokenizer)
        {
            while (tokenizer.Current is Keyword keyword && (
                       keyword == Constructor || keyword == Function || keyword == Method))
            {
                Tokens.Add(new SubroutineDeclaration(tokenizer));
            }
        }
        
        private void ClassVariableDeclarations(Tokenizer tokenizer)
        {
            while (tokenizer.Current is Keyword keyword && (keyword == Static || keyword == Field))
            {
                Tokens.Add(new ClassVariableDeclaration(tokenizer));
            }
        }

        public class SubroutineDeclaration : CompositeToken
        {
            public SubroutineDeclaration(Tokenizer tokenizer)
            {
                tokenizer
                    .CurrentIs(t => 
                        t is Keyword k && ( k == Constructor || k == Function || k == Method))
                    .ApplyThenMove(AddCurrent)
                    .CurrentIs(t =>
                        t is Keyword k && k == Keyword.Void ||TypeDeclaration(t))
                    .ApplyThenMove(AddCurrent)
                    .CurrentIsIdentifier()
                    .ApplyThenMove(AddCurrent)
                    .CurrentIs(Symbol.OpenParenthesis)
                    .ApplyThenMove(AddCurrent)
                    .Apply(t => Tokens.Add(new ParameterList(t)))
                    .CurrentIs(Symbol.CloseParenthesis)
                    .ApplyThenMove(AddCurrent)
                    .Apply(t => Tokens.Add(new SubroutineBody(t)));
            }
        }
        
        public class SubroutineBody : CompositeToken
        {
            public SubroutineBody(Tokenizer tokenizer)
            {
                tokenizer
                    .CurrentIs(Symbol.OpenCurly)
                    .ApplyThenMove(AddCurrent)
                    .ApplyWhile(Var, t => Tokens.Add(new VaribleDeclaration(t)))
                    .Apply(AddStatements)
                    .CurrentIs(Symbol.CloseCurly)
                    .ApplyThenMove(AddCurrent);
            }
        }
        
        public class VaribleDeclaration : CompositeToken
        {
            public VaribleDeclaration(Tokenizer tokenizer)
            {
                tokenizer
                    .CurrentIs(Var)
                    .Apply(AddCurrent) // VAR
                    .CurrentIs(TypeDeclaration)
                    .Apply(AddCurrent)
                    .CurrentIsIdentifier()
                    .Apply(AddCurrent)
                    .ApplyIf(Symbol.Commna, s => s
                        .Apply(AddCurrent)
                        .CurrentIsIdentifier()
                        .Apply(AddCurrent))
                    .CurrentIs(Symbol.SemiColon)
                    .Apply(AddCurrent);
            }
        }
        
        public class ParameterList : CompositeToken
        {
            public ParameterList(Tokenizer tokenizer)
            {
                while (!tokenizer.Current.Equals(Symbol.CloseParenthesis))
                {
                    tokenizer
                        .CurrentIs(TypeDeclaration)
                        .ApplyThenMove(AddCurrent)
                        .CurrentIsIdentifier()
                        .ApplyThenMove(AddCurrent)
                        .ApplyIf(Symbol.Commna, t => t
                            .ApplyThenMove(AddCurrent));
                }
            }
        }
        
        public class ClassVariableDeclaration : CompositeToken
        {
            public ClassVariableDeclaration(Tokenizer tokenizer)
            {
                tokenizer
                    .ApplyThenMove(AddCurrent) // STATIC OR FIELD
                    .CurrentIs(TypeDeclaration)
                    .ApplyThenMove(AddCurrent)
                    .CurrentIsIdentifier()
                    .ApplyThenMove(AddCurrent)
                    .ApplyWhile(Symbol.Commna, t => t
                        .ApplyThenMove(AddCurrent)
                        .CurrentIsIdentifier()
                        .ApplyThenMove(AddCurrent))
                    .CurrentIs(Symbol.SemiColon)
                    .ApplyThenMove(AddCurrent);
            }
        }

        private static bool TypeDeclaration(Token token)
        {
            return
                token is Identifier ||
                token is Keyword k && (
                    k == Int || k == Keyword.Char || k == Keyword.Boolean);
        }
    }
}
using System;
using System.Runtime.InteropServices.ComTypes;
using Analyzer.Tokens;
using static Analyzer.Tokens.KeywordToken;

namespace Analyzer
{
    public class JackProgram : CompositeToken
    {
        public JackProgram(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(Class)
                .ApplyThenMove(AddCurrent)
                .CurrentIs(SymbolToken.OpenCurly)
                .ApplyThenMove(AddCurrent)
                .Apply(ClassLevelDeclarations)
                .Apply(SubroutineDeclarations)
                .CurrentIs(SymbolToken.CloseCurly)
                .ApplyThenMove(AddCurrent);
        }

        private void SubroutineDeclarations(Tokenizer tokenizer)
        {
            while (tokenizer.Current is KeywordToken keyword && (
                       keyword == Constructor || keyword == Function || keyword == Method))
            {
                tokenizer
                    .ApplyThenMove(AddCurrent)
                    .CurrentIs(t =>
                        t is KeywordToken k && k == KeywordToken.Void ||
                        TypeDeclaration(t))
                    .ApplyThenMove(AddCurrent)
                    .CurrentIsIdentifier()
                    .ApplyThenMove(AddCurrent)
                    .CurrentIs(SymbolToken.OpenParenthesis)
                    .ApplyThenMove(AddCurrent)
                    .Apply(ParameterList)
                    .CurrentIs(SymbolToken.CloseParenthesis)
                    .Apply(SubroutineBody);
            }
        }

        private void SubroutineBody(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(SymbolToken.OpenCurly)
                .Apply(VaribleDeclarations)
                .Apply(AddStatements)
                .CurrentIs(SymbolToken.CloseCurly)
                .Apply(AddCurrent);
        }

        private void VaribleDeclarations(Tokenizer tokenizer)
        {
            tokenizer
                .ApplyWhile(Var, t => t
                    .Apply(AddCurrent) // VAR
                    .CurrentIs(TypeDeclaration)
                    .Apply(AddCurrent)
                    .CurrentIsIdentifier()
                    .Apply(AddCurrent)
                    .ApplyIf(SymbolToken.Commna, s => s
                        .Apply(AddCurrent)
                        .CurrentIsIdentifier()
                        .Apply(AddCurrent))
                    .CurrentIs(SymbolToken.SemiColon)
                    .Apply(AddCurrent));
        }

        private void ParameterList(Tokenizer tokenizer)
        {
            while (!tokenizer.Current.Equals(SymbolToken.CloseParenthesis))
            {
                tokenizer
                    .CurrentIs(TypeDeclaration)
                    .ApplyThenMove(AddCurrent)
                    .CurrentIsIdentifier()
                    .ApplyThenMove(AddCurrent);
            }
        }

        private void ClassLevelDeclarations(Tokenizer tokenizer)
        {
            while (tokenizer.Current is KeywordToken keyword && (
                       keyword == Static || keyword == Field))
            {
                tokenizer
                    .ApplyThenMove(AddCurrent) // STATIC OR FIELD
                    .CurrentIs(TypeDeclaration)
                    .ApplyThenMove(AddCurrent)
                    .CurrentIsIdentifier()
                    .ApplyThenMove(AddCurrent)
                    .ApplyWhile(SymbolToken.Commna, t => t
                        .ApplyThenMove(AddCurrent)
                        .CurrentIsIdentifier()
                        .ApplyThenMove(AddCurrent))
                    .CurrentIs(SymbolToken.SemiColon)
                    .ApplyThenMove(AddCurrent);
            }
        }

        private static bool TypeDeclaration(Token token)
        {
            return 
                token is IdentifierToken ||
                token is KeywordToken k && (
                    k == Int || k == KeywordToken.Char || k == KeywordToken.Boolean);
        }
    }
}
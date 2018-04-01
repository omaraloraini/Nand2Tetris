using Analyzer.Tokens;
using FluentAssertions;
using Xunit;

namespace Analyzer.Tests
{
    public class TokenizerTests
    {
        [Fact]
        public void Tokenize_WithAdjacentSymbols_IsCorrect()
        {
            var source = "(){}";

            Tokenizer.Tokenize(source)
                .Should()
                .HaveCount(4)
                .And
                .ContainInOrder(
                    SymbolToken.OpenParenthesis,
                    SymbolToken.CloseParenthesis,
                    SymbolToken.OpenCurly,
                    SymbolToken.CloseCurly
                    );
        }

        [Fact]
        public void Tokenize_WithString_IsCorrect()
        {
            var source = " \"hello\" ";

            Tokenizer.Tokenize(source)
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .Be(new StringToken("hello"));
        }

        [Fact]
        public void Tokenize_WithIdenitifers_IsCorrect()
        {
            const string idenitfier = "getx";
            
            var source = idenitfier;

            Tokenizer.Tokenize(source)
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .Be(new IdentifierToken(idenitfier));
        }

        [Fact]
        public void Tokenize_FunctionCall_ResolvedCorrectly()
        {
            var source = "do setx(5);";

            Tokenizer.Tokenize(source)
                .Should()
                .HaveCount(6)
                .And
                .ContainInOrder(
                    KeywordToken.Do,
                    new IdentifierToken("setx"),
                    SymbolToken.OpenParenthesis,
                    new IntegerToken(5),
                    SymbolToken.CloseParenthesis
                );
        }

        [Fact]
        public void Tokenize_NesterBlock_ResolvedCorrectly()
        {
            var source = "if (x < 100) { x = x * 2; }";

            Tokenizer.Tokenize(source)
                .Should()
                .HaveCount(14)
                .And
                .ContainInOrder(new Token[]
                {
                    KeywordToken.If,
                    SymbolToken.OpenParenthesis,
                    new IdentifierToken("x"),
                    SymbolToken.LessThan,
                    new IntegerToken(100),
                    SymbolToken.CloseParenthesis,
                    SymbolToken.OpenCurly,
                    new IdentifierToken("x"),
                    SymbolToken.Equal,
                    new IdentifierToken("x"),
                    SymbolToken.Star,
                    new IntegerToken(2),
                    SymbolToken.SemiColon,
                    SymbolToken.CloseCurly
                });
        }
    }
}
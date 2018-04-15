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
                    Symbol.OpenParenthesis,
                    Symbol.CloseParenthesis,
                    Symbol.OpenCurly,
                    Symbol.CloseCurly
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
                .Be(new StringConstant("hello"));
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
                .Be(new Identifier(idenitfier));
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
                    Keyword.Do,
                    new Identifier("setx"),
                    Symbol.OpenParenthesis,
                    new IntegerConstant(5),
                    Symbol.CloseParenthesis
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
                .ContainInOrder(
                    Keyword.If,
                    Symbol.OpenParenthesis,
                    new Identifier("x"),
                    Symbol.LessThan,
                    new IntegerConstant(100),
                    Symbol.CloseParenthesis,
                    Symbol.OpenCurly,
                    new Identifier("x"),
                    Symbol.Equal,
                    new Identifier("x"),
                    Symbol.Star,
                    new IntegerConstant(2), 
                    Symbol.SemiColon,
                    Symbol.CloseCurly);
        }
    }
}
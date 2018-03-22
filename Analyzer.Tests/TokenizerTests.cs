using FluentAssertions;
using Xunit;

namespace Analyzer.Tests
{
    public class TokenizerTests
    {
        [Theory]
        [InlineData("if", TokenType.Keyword, TokenName.If)]
        [InlineData("class", TokenType.Keyword, TokenName.Class)]
        public void Tokenize_Keywords_GetResolvedWithCorrectTokenTypeAndName(
            string source, TokenType type, TokenName name)
        {
            var tokens = Tokenizer.Tokenize(source);

            var subject = tokens
                .Should()
                .ContainSingle()
                .Subject;

            subject
                .Name.Should().Be(name);

            subject
                .Type.Should().Be(type);
        }
        
        [Theory]
        [InlineData("(", TokenType.Symbol, TokenName.OpenParenthesis)]
        [InlineData("=", TokenType.Symbol, TokenName.Equal)]
        [InlineData(".", TokenType.Symbol, TokenName.Dot)]
        public void Tokenize_Symbols_GetResolvedWithCorrectTokenTypeAndName(
            string source, TokenType type, TokenName name)
        {
            var tokens = Tokenizer.Tokenize(source);

            var subject = tokens
                .Should()
                .ContainSingle()
                .Subject;

            subject
                .Name.Should().Be(name);

            subject
                .Type.Should().Be(type);
        }

        [Fact]
        public void Tokenize_WithAdjacentSymbols_IsCorrect()
        {
            var source = "(){}";

            Tokenizer.Tokenize(source)
                .Should()
                .HaveCount(4)
                .And
                .ContainInOrder(
                    Token.Symbol('('),
                    Token.Symbol(')'),
                    Token.Symbol('{'),
                    Token.Symbol('}'));
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
                .Be(Token.StringConsant("hello"));
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
                .Be(Token.Parse(idenitfier));
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
                    Token.Parse("do"),
                    Token.Parse("setx"),
                    Token.Symbol('('),
                    Token.IntegerConstant(5),
                    Token.Symbol(')')
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
                .StartWith(new[]
                {
                    Token.Parse("if"), 
                    Token.Symbol('(') 
                })
                .And
                .EndWith(new[]
                {
                    Token.Symbol(';'),
                    Token.Symbol('}')       
                });
        }
    }
}
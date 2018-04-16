using Analyzer.Expressions;
using Analyzer.Statements;
using Analyzer.Tokens;
using FluentAssertions;
using Xunit;

namespace Analyzer.Tests
{
    public class StatementsTests
    {
        [Fact]
        public void LetStatement_Identifier_IsCorrect()
        {
            var tokenizer = new Tokenizer(new Token[]
            {
                Keyword.Let,
                new Identifier("x"),
                Symbol.Equal,
                new IntegerConstant(5),
                Symbol.SemiColon
            });

            var letStatement = new LetStatement(tokenizer);

            letStatement
                .Varible.Should().BeOfType<Identifier>()
                .Which.Value.Should().Be("x");

            letStatement
                .Expression.Term.Should().BeOfType<IntegerConstant>()
                .Which.Integer.Should().Be(5);
        }

        [Fact]
        public void LetStatement_ArrayIdentifier_IsCorrect()
        {
            var tokenizer = new Tokenizer(new Token[]
            {
                Keyword.Let,
                new Identifier("a"),
                Symbol.OpenBracket,
                new IntegerConstant(1),
                Symbol.Plus,
                new IntegerConstant(1), 
                Symbol.CloseBracket,
                Symbol.Equal,
                new IntegerConstant(7),
                Symbol.SemiColon
            });

            var letStatement = new LetStatement(tokenizer);

            letStatement
                .Varible.Should().BeOfType<ArrayIdentifier>()
                .Which.Invoking(a =>
                {
                    a.Identifier.Value.Should().Be("a");
                    a.Expression.Term.Should().BeOfType<IntegerConstant>()
                        .Which.Integer.Should().Be(1);
                    
                    var tuple = a.Expression.OperatorTermPairs.Should().ContainSingle()
                        .Which;

                    tuple.Item1.Should().Be(Symbol.Plus);
                    tuple.Item2.Should().BeOfType<IntegerConstant>()
                        .Which.Integer.Should().Be(1);
                })
                .Invoke();

            letStatement
                .Expression.Term.Should().BeOfType<IntegerConstant>()
                .Which.Integer.Should().Be(7);

            letStatement.Expression.OperatorTermPairs.Should().BeEmpty();
        }

        [Fact]
        public void IfStatment_isCorrect()
        {
            var tokenizer = new Tokenizer(new Token[]
            {
                Keyword.If,
                Symbol.OpenParenthesis,
                new Identifier("x"),
                Symbol.LessThan,
                new IntegerConstant(10), 
                Symbol.CloseParenthesis,
                Symbol.OpenCurly,
                Keyword.Let,
                new Identifier("x"),
                Symbol.Equal,
                new Identifier("x"), 
                Symbol.Plus,
                new IntegerConstant(1),
                Symbol.SemiColon,
                Symbol.CloseCurly
            });

            var ifStatement = new IfStatement(tokenizer);

            ifStatement
                .Condition.Term
                .Should().BeOfType<Identifier>().Which.Value.Should().Be("x");

            ifStatement
                .Condition.OperatorTermPairs.Should().ContainSingle();
            
            ifStatement
                .TrueBranch.Should().ContainSingle()
                .Which.Should().BeOfType<LetStatement>()
                .Which.Invoking(let =>
                {
                    let.Varible.Should().BeOfType<Identifier>().Which.Value.Should().Be("x");
                    let.Expression.Term.Should().BeOfType<Identifier>().Which.Value.Should().Be("x");
                    var tuple = let.Expression.OperatorTermPairs.Should().ContainSingle().Which;
                    tuple.Item1.Should().Be(Symbol.Plus);
                    tuple.Item2.Should().BeOfType<IntegerConstant>().Which.Integer.Should().Be(1);
                })
                .Invoke();

            ifStatement.FalseBranch.Should().BeEmpty();
        }
    }
}
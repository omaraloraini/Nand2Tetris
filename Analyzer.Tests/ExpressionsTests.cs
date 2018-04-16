using System;
using System.Linq;
using Analyzer.Expressions;
using Analyzer.Tokens;
using FluentAssertions;
using Xunit;

namespace Analyzer.Tests
{
    public class ExpressionsTests
    {
        [Fact]
        public void Expression_IntegerConstantToken_IsCorrect()
        {
            var expression = new Expression(new Tokenizer(new[]
            {
                new IntegerConstant(1)
            }));

            expression.Term.Should()
                .BeOfType<IntegerConstant>()
                .Which.Integer.Should().Be(1);

            expression.OperatorTermPairs
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Expression_StringConstant_IsCorrect()
        {
            var expression = new Expression(new Tokenizer(new[]
            {
                new StringConstant("Hello"), 
            }));

            expression.Term.Should()
                .BeOfType<StringConstant>()
                .Which.Value.Should().Be("Hello");

            expression.OperatorTermPairs
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Expression_KeywordConstant_IsCorrect()
        {
            var expression = new Expression(new Tokenizer(new[]
            {
                KeywordConstant.True 
            }));

            expression.Term.Should()
                .BeOfType<KeywordConstant>()
                .Which.Should().Be(KeywordConstant.True);

            expression.OperatorTermPairs
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Expression_Identifier_IsCorrect()
        {
            var expression = new Expression(new Tokenizer(new[]
            {
                new Identifier("x") 
            }));

            expression.Term.Should()
                .BeOfType<Identifier>()
                .Which.Should().Be(new Identifier("x"));

            expression.OperatorTermPairs
                .Should()
                .BeEmpty();
        }
        
        [Fact]
        public void Expression_WithInParentheses_IsCorrect()
        {
            var expression = new Expression(new Tokenizer(new Token[]
            {
                Symbol.OpenParenthesis,
                new IntegerConstant(5),
                Symbol.CloseParenthesis
            }));

            expression.Term.Should()
                .BeOfType<Expression>()
                .Which
                .Term.Should().Be(new IntegerConstant(5));

            expression.OperatorTermPairs
                .Should()
                .BeEmpty();
        }
        
        [Fact]
        public void Expression_UnaryTerm_IsCorrect()
        {
            var expression = new Expression(new Tokenizer(new Token[]
            {
                Symbol.Minus,
                new IntegerConstant(5)
            }));

            expression.Term.Should()
                .BeOfType<UnaryTerm>()
                .Which.Should().Be(new UnaryTerm(Symbol.Minus, new IntegerConstant(5)));

            expression.OperatorTermPairs
                .Should()
                .BeEmpty();
        }
        
        [Fact]
        public void Expression_ArrayIdentifier_IsCorrect()
        {
            var expression = new Expression(new Tokenizer(new Token[]
            {
                new Identifier("arr"),
                Symbol.OpenBracket,
                new IntegerConstant(1),
                Symbol.CloseBracket
            }));

            var assertion = expression.Term.Should()
                .BeOfType<ArrayIdentifier>();

            assertion.Which.Identifier.Should().Be(new Identifier("arr"));
            assertion.Which.Expression.Term.Should().Be(new IntegerConstant(1));

            expression.OperatorTermPairs
                .Should()
                .BeEmpty();
        }

        [Fact (Skip = "ValueTuple bug or FA bug")]
        public void Expression_MultipleOprators_IsCorrect()
        {
            var expression = new Expression(new Tokenizer(new Token[]
            {
                new Identifier("x"),
                Symbol.Plus,
                new IntegerConstant(2),
                Symbol.Star,
                new Identifier("y")
            }));

            expression
                .Term.Should().Be(new Identifier("x"));

            expression
                .OperatorTermPairs
                .Should()
                .ContainInOrder(
                    (Symbol.Plus, new IntegerConstant(2)),
                    (Symbol.Star, new Identifier("y")));
        }

        [Fact]
        public void Expression_SubroutineCall_IsCorrect()
        {
            var expression = new Expression(new Tokenizer(new Token[]
            {
                new Identifier("x"),
                Symbol.OpenParenthesis,
                new IntegerConstant(1),
                Symbol.Commna,
                new IntegerConstant(2),
                Symbol.CloseParenthesis
            }));

            expression
                .Term.Should().BeOfType<SubroutineCall>()
                .Which.Invoking(call =>
                {
                    call.CalledOn.Should().BeNull();
                    call.SobroutineName.Should().Be(new Identifier("x"));
                    call.Parametars
                        .Select(e => e.Term)
                        .Should().AllBeOfType<IntegerConstant>()
                        .And.ContainInOrder(new IntegerConstant(1), new IntegerConstant(2));

                })
                .Invoke();
        }
        
        [Fact]
        public void Expression_SubroutineCallOnAnObject_IsCorrect()
        {
            var expression = new Expression(new Tokenizer(new Token[]
            {
                new Identifier("x"),
                Symbol.Dot,
                new Identifier("y"), 
                Symbol.OpenParenthesis,
                Symbol.CloseParenthesis
            }));

            expression
                .Term.Should().BeOfType<SubroutineCall>()
                .Which.Invoking(call =>
                {
                    call.CalledOn.Should().Be(new Identifier("x"));
                    call.SobroutineName.Should().Be(new Identifier("y"));
                    call.Parametars.Should().BeEmpty();
                })
                .Invoke();
        }
    }
}
using Analyzer.ProgramStructures;
using Analyzer.Tokens;
using FluentAssertions;
using Xunit;

namespace Analyzer.Tests
{
    public class SubroutineTests
    {
        [Fact]
        public void Parse_NoParamtersAndEmptyBody_IsCorrect()
        {
            var subroutine = Subroutine.Parse(new Tokenizer(new Token[]
            {
                Keyword.Function,
                Keyword.Void,
                new Identifier("f"),
                Symbol.OpenParenthesis,
                Symbol.CloseParenthesis,
                Symbol.OpenCurly,
                Symbol.CloseCurly
            }));

            subroutine.Name.Value.Should().Be("f");
            subroutine.ReturnType.Name.Should().Be("void");
            subroutine.Arguments.Should().BeEmpty();
            subroutine.LocalVaribles.Should().BeEmpty();
            subroutine.Statements.Should().BeEmpty();
        }

        [Fact]
        public void Parse_WithParamtersAndEmptyBody_IsCorrect()
        {
            var subroutine = Subroutine.Parse(new Tokenizer(new Token[]
            {
                Keyword.Function,
                new Identifier("Point"),
                new Identifier("f"),
                Symbol.OpenParenthesis,
                new Identifier("Point"),
                new Identifier("p"),
                Symbol.Commna,
                Keyword.Int,
                new Identifier("i"),
                Symbol.CloseParenthesis,
                Symbol.OpenCurly,
                Symbol.CloseCurly
            }));

            subroutine.Name.Value.Should().Be("f");
            subroutine.ReturnType.Name.Should().Be("Point");
            subroutine.LocalVaribles.Should().BeEmpty();
            subroutine.Statements.Should().BeEmpty();
            subroutine.Arguments.Should().ContainInOrder(
                new Varible(new VaribleType(new Identifier("Point")), new Identifier("p")),
                new Varible(new VaribleType(Keyword.Int), new Identifier("i"))
            );
        }

        [Fact]
        public void Parse_WithTwoStatments_IsCorrect()
        {
            var subroutine = Subroutine.Parse(new Tokenizer(new Token[]
            {
                Keyword.Function,
                new Identifier("Point"),
                new Identifier("Create"),
                Symbol.OpenParenthesis,
                Symbol.CloseParenthesis,
                Symbol.OpenCurly,
                
                Keyword.Var,
                new Identifier("Point"),
                new Identifier("p"),
                Symbol.SemiColon,
                
                Keyword.Let,
                new Identifier("p"),
                Symbol.Equal,
                new Identifier("Point"),
                Symbol.Dot,
                new Identifier("new"),
                Symbol.OpenParenthesis,
                new IntegerConstant(0),
                Symbol.Commna,
                new IntegerConstant(0),
                Symbol.CloseParenthesis,
                Symbol.SemiColon,
                
                Keyword.Return,
                new Identifier("p"),
                Symbol.SemiColon,
                
                Symbol.CloseCurly
            }));
            
            subroutine.Name.Value.Should().Be("Create");
            subroutine.ReturnType.Name.Should().Be("Point");
            subroutine.Arguments.Should().BeEmpty();
            subroutine.LocalVaribles.Should().ContainSingle(v =>
                v.VaribleType.Name == "Point" && v.Identifier.Value == "p");

            subroutine.Statements.Should().HaveCount(2);
        }
    }
}
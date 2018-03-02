using Xunit;

namespace VirtualMachine.Tests
{
    public class ArithmeticCommandTests
    {
        [Fact]
        public void Add_IsCorrect()
        {
            var actual = ArithmeticCommand.Add().HackInstructions;

            var expected = new[]
            {
                "@SP",
                "M=M-1",
                "A=M",
                "D=M",
                "A=A-1",
                "M=D+M"
            };
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sub_IsCorrect()
        {
            var actual = ArithmeticCommand.Sub().HackInstructions;

            var expected = new[]
            {
                "@SP",
                "M=M-1",
                "A=M",
                "D=-M",
                "A=A-1",
                "M=D+M"
            };
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Neg_IsCorrect()
        {
            var actual = ArithmeticCommand.Neg().HackInstructions;

            var expected = new[]
            {
                "@SP",
                "A=M-1",
                "M=-M"
            };
            
            Assert.Equal(expected, actual);
        }
    }
}
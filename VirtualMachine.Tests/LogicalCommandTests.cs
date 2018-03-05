using Xunit;

namespace VirtualMachine.Tests
{
    public class LogicalCommandTests
    {
        [Fact]
        public void And_IsCorrect()
        {
            var actual = Comparsion.And().HackInstructions;

            var expected = new[]
            {
                "@SP",
                "M=M-1",
                "A=M",
                "D=M",
                "A=A-1",
                "M=D&M"
            };
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Or_IsCorrect()
        {
            var actual = Comparsion.Or().HackInstructions;

            var expected = new[]
            {
                "@SP",
                "M=M-1",
                "A=M",
                "D=M",
                "A=A-1",
                "M=D|M"
            };
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Not_IsCorrect()
        {
            var actual = Comparsion.Not().HackInstructions;

            var expected = new[]
            {
                "@SP",
                "A=M-1",
                "M=!M"
            };
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Equal_IsCorrect()
        {
            const string label = "XX";
            
            var actual = Comparsion.Equal(label).HackInstructions;

            var expected = new[]
            {
                "@SP",
                "M=M-1",
                "A=M",
                "D=M",
                "A=A-1",
                "D=D-M",
                "M=-1",
                $"@{label}",
                "D;JEQ",
                "@SP",
                "A=M-1",
                "M=0",
                $"({label})"
            };
            
            Assert.Equal(expected, actual);
        }
    }
}
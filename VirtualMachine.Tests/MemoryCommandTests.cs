using Xunit;

namespace VirtualMachine.Tests
{
    public class MemoryCommandTests
    {
        [Theory]
        [InlineData(2)]
        [InlineData(0)]
        [InlineData(-3000)]
        public void Push_Constant_IsCorrect(int i)
        {
            var actual = MemoryCommand.Push("", "constant", i).HackInstructions;

            var expected = new[]
            {
                $"@{i}",
                "D=A",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D",
            };
            
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("local", "@LCL", 0)]
        [InlineData("argument", "@ARG" ,1)]
        [InlineData("that", "@THAT", 2)]
        [InlineData("this", "@THIS" ,2)]
        public void Push_ConstantLocalArgumentThisThat_IsCorrect(
            string segment, string segmentBase, int index)
        {
            var actual = MemoryCommand.Push("", segment, index).HackInstructions;

            var expected = new[]
            {
                segmentBase,
                "D=M",
                $"@{index}",
                "A=D+A",
                "D=M",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D",
            };
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Push_StaticSegment_IsCorrect()
        {
            var actual = MemoryCommand.Push("Foo", "static", 2).HackInstructions;
            
            var expected = new[]
            {
                "@Foo.2",
                "D=M",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D",
            };
            
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(2)]
        public void Push_Temp_IsCorrect(int index)
        {
            var actual = MemoryCommand.Push("", "temp", index).HackInstructions;

            var expected = new[]
            {
                $"@{index + 5}",
                "D=M",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D"
            };
            
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, "@THIS")]
        [InlineData(1, "@THAT")]
        public void Push_Pointer_IsCorrect(int index, string segment)
        {
            var actual = MemoryCommand.Push("", "pointer", index).HackInstructions;

            var expected = new[]
            {
                segment,
                "D=M",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D"
            };
            
            Assert.Equal(expected, actual);
        }
    }
}
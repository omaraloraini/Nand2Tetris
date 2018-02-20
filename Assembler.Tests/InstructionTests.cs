using System;
using Xunit;

namespace Assembler.Tests
{
    public class InstructionTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(77)]
        public void Instruction_A_ReturnsCorrectBinary(int instruction)
        {
            var actual = Instruction.A(instruction).ToBinary();

            var expected = Convert.ToString(instruction, 2).PadLeft(16, '0');

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("0", "M", "", "1110101010001000")]
        [InlineData("M", "D", "", "1111110000010000")]
        [InlineData("D", "", "JEQ", "1110001100000010")]
        [InlineData("0", "", "JMP", "1110101010000111")]
        public void Instruction_C_RetrunsCorrectBinary(string comp, string dest, string jump,
            string expected)
        {
            var actual = Instruction.C(comp, dest, jump).ToBinary();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("@1", "0000000000000001")]
        [InlineData("@5", "0000000000000101")]
        [InlineData("@1447", "0000010110100111")]
        [InlineData("M=0", "1110101010001000")]
        [InlineData("D=0", "1110101010010000")]
        [InlineData("D;JEQ", "1110001100000010")]
        [InlineData("0;JMP", "1110101010000111")]
        [InlineData("A=D-1;JEQ", "1110001110100010")]
        public void Instruction_Parse_ReturnsCorrectBinary(string instruction, string expected)
        {
            var actual = Instruction.Parse(instruction).ToBinary();

            Assert.Equal(expected, actual);
        }
    }
}
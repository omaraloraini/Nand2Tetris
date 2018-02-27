using System;
using System.IO;
using Xunit;

namespace Assembler.Tests
{
    public class HackAssemblerTests
    {
        [Fact]
        public void Asseble_WithEmptyLinesAndCommentsAndWhiteSpaces_AreIgnored()
        {
            var lines = new[]
            {
                "",
                "// comment",
                "   ",
                "@0  ",
                "@1 // comment"
            };

            var expected = new[]
            {
                "0000000000000000",
                "0000000000000001"
            };

            var actual = HackAssembler.Assemble(lines);
            
            Assert.Equal(expected, actual);
        }

        
        [Theory]
        [InlineData("@SP", new[] {"0000000000000000"})]
        [InlineData("@LCL", new[] {"0000000000000001"})]
        [InlineData("@ARG", new[] {"0000000000000010"})]
        [InlineData("@THIS", new[] {"0000000000000011"})]
        [InlineData("@THAT", new[] {"0000000000000100"})]
        [InlineData("@SCREEN", new[] {"0100000000000000"})]
        [InlineData("@KBD", new[] {"0110000000000000"})]
        [InlineData("@R0", new[] {"0000000000000000"})]
        [InlineData("@R15", new[] {"0000000000001111"})]
        public void Assemble_SymbolsAreResolved_Correctly(string symbol, string[] expected)
        {
            var lines = new []
            {
                symbol
            };

            var actual = HackAssembler.Assemble(lines);
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Asseble_LoacalsAreResolved_Correctly()
        {
            var lines = new []
            {
                "@i",
                "@longName",
                "@i",
                "@x"
            };

            var expcted = new[]
            {
                "0000000000010000",
                "0000000000010001",
                "0000000000010000",
                "0000000000010010",
            };

            var actual = HackAssembler.Assemble(lines);
            
            Assert.Equal(expcted, actual);
        }

        [Fact]
        public void Assemble_LabelsAreResolved_Correctly()
        {
            var lines = new []
            {
                "(FIRST)",
                "@0",
                "(SECOND)",
                "@i",
                "(THIRD)",
                "@x",
                "@FIRST",
                "@THIRD"
            };

            var expcted = new[]
            {
                "0000000000000000",
                "0000000000010000",
                "0000000000010001",
                "0000000000000000",
                "0000000000000010",
                
            };

            var actaul = HackAssembler.Assemble(lines);
            
            Assert.Equal(expcted, actaul);
        }
        
        [Fact]
        public void Assemble_ReturnsCorrectBinaryInsertuctions()
        {
            var lines = new[] {
                "",
                "// This file is part of www.nand2tetris.org",
                "// and the book \"The Elements of Computing Systems\"",
                "// by Nisan and Schocken, MIT Press.",
                "// File name: projects/04/Mult.asm",
                "",
                "// Multiplies R0 and R1 and stores the result in R2.",
                "// (R0, R1, R2 refer to RAM[0], RAM[1], and RAM[2], respectively.)",
                "",
                "// Put your code here.",
                "@R2",
                "    M=0",
                "@i",
                "    M=0",
                "        (LOOP)",
                "@R0",
                "    D=M",
                "@i",
                "    D=D-M",
                "@END",
                "    D;JEQ",
                "",
                "    @R1",
                "D=M",
                "    @R2",
                "M=D+M",
                "    @i",
                "M=M+1",
                "@LOOP",
                "0;JMP",
                "",
                "    (END)",
                "@END",
                "0;JMP",
            };


            var expected = new[] {
                "0000000000000010",
                "1110101010001000",
                "0000000000010000",
                "1110101010001000",
                "0000000000000000",
                "1111110000010000",
                "0000000000010000",
                "1111010011010000",
                "0000000000010010",
                "1110001100000010",
                "0000000000000001",
                "1111110000010000",
                "0000000000000010",
                "1111000010001000",
                "0000000000010000",
                "1111110111001000",
                "0000000000000100",
                "1110101010000111",
                "0000000000010010",
                "1110101010000111",
            };

            var actual = HackAssembler.Assemble(lines);

            Assert.Equal(expected, actual);
        }
    }
}
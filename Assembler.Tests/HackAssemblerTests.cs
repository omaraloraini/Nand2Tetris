using System;
using System.IO;
using Xunit;

namespace Assembler.Tests
{
    public class HackAssemblerTests
    {
        [Fact]
        public void Assemble_ReturnsCorrectBinaryInsertuctions()
        {
            var lines = _asm.Split('\n');

            var expected = _hack.Split('\n');

            var actual = HackAssembler.Assemble(lines);

            Assert.Equal(expected.Length, actual.Length);

            Assert.Equal(expected, actual);
        }

        private static string _asm = @"
// This file is part of www.nand2tetris.org
// and the book ""The Elements of Computing Systems""
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Mult.asm

// Multiplies R0 and R1 and stores the result in R2.
// (R0, R1, R2 refer to RAM[0], RAM[1], and RAM[2], respectively.)

// Put your code here.
@R2
    M=0
@i
    M=0
        (LOOP)
@R0
    D=M
@i
    D=D-M
@END
    D;JEQ

    @R1
D=M
    @R2
M=D+M
    @i
M=M+1
@LOOP
0;JMP

    (END)
@END
0;JMP";

        private static string _hack =
            @"0000000000000010
1110101010001000
0000000000010000
1110101010001000
0000000000000000
1111110000010000
0000000000010000
1111010011010000
0000000000010010
1110001100000010
0000000000000001
1111110000010000
0000000000000010
1111000010001000
0000000000010000
1111110111001000
0000000000000100
1110101010000111
0000000000010010
1110101010000111";
    }
}
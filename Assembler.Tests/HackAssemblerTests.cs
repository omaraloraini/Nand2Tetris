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
            var lines = File.ReadAllLines("/Users/omaraloraini/RiderProjects/Nand2Tetris/Assembler.Tests/Mult.asm");

            var expected = File.ReadAllLines("/Users/omaraloraini/RiderProjects/Nand2Tetris/Assembler.Tests/Mult.hack");

            var actual = HackAssembler.Assemble(lines);
            
            Assert.Equal(expected, actual);            
        }
    }
}
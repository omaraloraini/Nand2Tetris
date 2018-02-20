using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assembler
{
    public class Instruction
    {
        private static Dictionary<string, BitArray> _despMap = new Dictionary<string, BitArray>
        {
            [""] = 0b000_000.To16Bit(),
            ["M"] = 0b001_000.To16Bit(),
            ["D"] = 0b010_000.To16Bit(),
            ["MD"] = 0b011_000.To16Bit(),
            ["A"] = 0b100_000.To16Bit(),
            ["AM"] = 0b101_000.To16Bit(),
            ["AD"] = 0b110_000.To16Bit(),
            ["AMD"] = 0b111_000.To16Bit(),
        };

        private static Dictionary<string, BitArray> _jumpMap = new Dictionary<string, BitArray>
        {
            [""] = 0b000.To16Bit(),
            ["JGT"] = 0b001.To16Bit(),
            ["JEQ"] = 0b010.To16Bit(),
            ["JGE"] = 0b011.To16Bit(),
            ["JLT"] = 0b100.To16Bit(),
            ["JNE"] = 0b101.To16Bit(),
            ["JLE"] = 0b110.To16Bit(),
            ["JMP"] = 0b111.To16Bit()
        };

        private static Dictionary<string, BitArray> _compMap = new Dictionary<string, BitArray>
        {
            ["0"] = 0b0_101010_000000.To16Bit(),
            ["1"] = 0b0_111111_000000.To16Bit(),
            ["-1"] = 0b0_111010_000000.To16Bit(),
            ["D"] = 0b0_001100_000000.To16Bit(),
            ["A"] = 0b0_110000_000000.To16Bit(),
            ["M"] = 0b1_110000_000000.To16Bit(),
            ["!D"] = 0b0_001101_000000.To16Bit(),
            ["!A"] = 0b0_110001_000000.To16Bit(),
            ["!M"] = 0b1_110001_000000.To16Bit(),
            ["-D"] = 0b0_001111_000000.To16Bit(),
            ["-A"] = 0b0_110011_000000.To16Bit(),
            ["-M"] = 0b1_110011_000000.To16Bit(),
            ["D+1"] = 0b0_011111_000000.To16Bit(),
            ["A+1"] = 0b0_110111_000000.To16Bit(),
            ["M+1"] = 0b1_110111_000000.To16Bit(),
            ["D-1"] = 0b0_001110_000000.To16Bit(),
            ["A-1"] = 0b0_110010_000000.To16Bit(),
            ["M-1"] = 0b1_110010_000000.To16Bit(),
            ["D+A"] = 0b0_000010_000000.To16Bit(),
            ["D+M"] = 0b1_000010_000000.To16Bit(),
            ["D-A"] = 0b0_010011_000000.To16Bit(),
            ["D-M"] = 0b1_010011_000000.To16Bit(),
            ["A-D"] = 0b0_000111_000000.To16Bit(),
            ["M-D"] = 0b1_000111_000000.To16Bit(),
            ["D&A"] = 0b0_000000_000000.To16Bit(),
            ["D&M"] = 0b1_000000_000000.To16Bit(),
            ["D|A"] = 0b0_010101_000000.To16Bit(),
            ["D|M"] = 0b1_010101_000000.To16Bit(),
        };

        private readonly BitArray _bitArray;

        private Instruction(BitArray bitArray)
        {
            _bitArray = bitArray;
        }

        public static Instruction A(int number)
        {
            return new Instruction(number.To16Bit());
        }

        public static Instruction C(string comp, string dest = "", string jump = "")
        {
            var bitArray = new BitArray(16, false);
            bitArray.Set(0, true);
            bitArray.Set(1, true);
            bitArray.Set(2, true);

            return new Instruction(
                bitArray
                    .Or(_compMap[comp])
                    .Or(_despMap[dest])
                    .Or(_jumpMap[jump]));
        }
        
        public static Instruction Parse(string instruction)
        {
            if (instruction.StartsWith('@'))
            {
                return A(int.Parse(instruction.Substring(1)));
            }
            
            var parts = instruction.Split('=', ';');
            if (parts.Length == 3)
            {
                return C(parts[1], parts[0], parts[2]);
            }

            if (instruction.Contains("="))
            {
                return C(comp: parts[1], dest: parts[0]);
            }

            if (instruction.Contains(";"))
            {
                return C(comp: parts[0], jump: parts[1]);
            }

            return C(parts[0]);
        }

        public int ToDecimal()
        {
            var result = new short[1];
            _bitArray.CopyTo(result, 0);
            return result[0];
        }

        public string ToBinary()
        {
            return string.Join("",
                _bitArray
                    .Cast<bool>()
                    .Select(b => b ? '1' : '0'));
        }

        public override string ToString() => ToBinary();
    }
}
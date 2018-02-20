using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assembler
{
    public class HackAssembler
    {
        private static Dictionary<string, string> _preDefinedSymbols = new Dictionary<string, string>
        {
            ["@SP"] = "@0",
            ["@LCL"] = "@1",
            ["@ARG"] = "@2",
            ["@THIS"] = "@3",
            ["@THAT"] = "@4",
            ["@SCREEN"] = "@16384",
            ["@KBD"] = "@24576",
        };

        static HackAssembler()
        {
            for (var i = 0; i < 16; i++) _preDefinedSymbols.Add($"@R{i}", $"@{i}");
        }

        public static string[] Assemble(string[] lines)
        {
            var instructions = lines
                .Select(line => line.Contains("//") 
                    ? line.Remove(line.IndexOf("//")).Replace(" ","")
                    : line.Replace(" ", ""))
                .Where(line => line != string.Empty)
                .ToList();

            var symbols = new Dictionary<string, string>();

            for (var i = 0; i < instructions.Count; i++)
            {
                var instruction = instructions[i];
                if (instruction.StartsWith('('))
                {
                    var symbol = instruction.Substring(1, instruction.Length - 2);
                    symbols.Add($"@{symbol}", $"@{i}");
                    instructions.RemoveAt(i);
                    i = i - 1;
                }
            }
            
            var currentAddress = 16;
            foreach (var instruction in instructions)
            {     
                if (instruction.StartsWith('@') && !IsInt(instruction.Substring(1)))
                {
                    if (_preDefinedSymbols.ContainsKey(instruction)) continue;
                    if (symbols.ContainsKey(instruction)) continue;
                    symbols.Add(instruction, $"@{currentAddress++}");
                }
            }

            return instructions
                .Select(instruction =>
                {
                    if (!instruction.StartsWith('@')) return instruction;

                    if (_preDefinedSymbols.ContainsKey(instruction))
                        return _preDefinedSymbols[instruction];

                    if (symbols.ContainsKey(instruction))
                        return symbols[instruction];

                    return instruction;
                })
                .Select(line => Instruction.Parse(line).ToBinary())
                .ToArray();

            bool IsInt(string number) => number.All(c => c >= '0' && c <= '9');
        }
    }
}
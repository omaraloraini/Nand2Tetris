using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualMachine
{
    internal class HackTranslator
    {
        public static IEnumerable<string> Translate(string fileName, string[] lines)
        {
            foreach (var instruction in ExtractCommands(lines).SelectMany(line => 
                Command.Parse(fileName, line, new LabelGenerator(fileName)).HackInstructions))
            {
                yield return instruction;
            }
            
            foreach (var instruction in ReturnFunction())
                yield return instruction;

            foreach (var instruction in FunctionCallFunction())
                yield return instruction;
        }
        
        public static IEnumerable<string> Translate(Dictionary<string, string[]> files)
        {
            return Bootstrapper()
                .Concat(ReturnFunction())
                .Concat(FunctionCallFunction())
                .Concat(files.SelectMany(file => TranslateFile(file.Key,file.Value)));

            IEnumerable<string> TranslateFile(string fileName, string[] lines)
            {
                var queue = new Queue<string>(ExtractCommands(lines));
                while (queue.Count != 0)
                {
                    foreach (var instruction in Function.Parse(fileName, queue).HackInstructions)
                        yield return instruction;
                }
            }
        }

        private static IEnumerable<string> ExtractCommands(IEnumerable<string> lines) =>
            lines
                .Where(line => line != string.Empty && !line.StartsWith("//"))
                .Select(line => line.Contains("//")
                    ? line.Remove(line.IndexOf("//", StringComparison.Ordinal)).Trim()
                    : line.Trim());

        private static IEnumerable<string> Bootstrapper()
        {
            var labelGenerator = new LabelGenerator("bootstrapper");
            return new[]
            {
                "@256",
                "D=A",
                "@SP",
                "M=D"
            }.Concat(Commands
                .FunctionCall("Sys.init", 0, labelGenerator).HackInstructions);
        }

        private static IEnumerable<string> ReturnFunction()
        {
            return new[]
            {
                "(RETURN)",
                // R13 = ENDFRAME
                "@LCL",
                "D=M",
                "@R13",
                "M=D",

                // R14 = RETURN ADDRESS
                "@5",
                "A=D-A",
                "D=M",
                "@R14",
                "M=D",

                // *ARG = POP
                "@SP",
                "A=M-1",
                "D=M",
                "@ARG",
                "A=M",
                "M=D",

                // SP = ARG + 1
                "D=A+1",
                "@SP",
                "M=D",

                "@R13",
                "AM=M-1",
                "D=M",
                "@THAT",
                "M=D",

                "@R13",
                "AM=M-1",
                "D=M",
                "@THIS",
                "M=D",

                "@R13",
                "AM=M-1",
                "D=M",
                "@ARG",
                "M=D",

                "@R13",
                "AM=M-1",
                "D=M",
                "@LCL",
                "M=D",

                "@R14",
                "A=M",
                "0;JMP"
            };
        }

        private static IEnumerable<string> FunctionCallFunction()
        {
            return new[]
            {
                "(CALL)",
                "@R13",
                "A=M",
                "D=A",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D",

                "@LCL",
                "D=M",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D",

                "@ARG",
                "D=M",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D",

                "@THIS",
                "D=M",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D",

                "@THAT",
                "D=M",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D",


                "D=A+1",
                "@LCL",
                "M=D",

                "@5",
                "D=D-A",
                "@R14",
                "A=M",
                "D=D-A",
                "@ARG",
                "M=D",

                "@R15",
                "A=M",
                "0;JMP",
            };
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace VirtualMachine
{
    public class FunctionCommand : Command
    {
        private FunctionCommand(IEnumerable<string> hackInstructions) : base(hackInstructions)
        {
        }

        private static int counter = 0;
        public static Command Call(string fileName, string functionName, int numberOfArguments)
        {
            var label = $"ret${counter++}";
            return new FunctionCommand(new[]
            {
                "@" + label,
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
                $"@{numberOfArguments}",
                "D=D-A",
                "@ARG",
                "M=D",

                $"@{functionName}",
                "0;JMP",

                $"({label})"
            });
        }

        public static Command DeclareFunction(string functionName, int numberOfParameters)
        {
            return Label(functionName)
                .Combine(Enumerable
                    .Repeat(0, numberOfParameters)
                    .Select(_ => MemoryCommand.Push("", "constant", 0)));
        }

        public static Command CreateFunction(string fileName,
            string functionName, int numberOfParameters, IEnumerable<string> commands)
        {
            return DeclareFunction(functionName, numberOfParameters)
                .Combine(commands.Select(command => Parse(fileName, command)))
                .Combine(Retrun());
        }

        public static Command Retrun()
        {
            return new FunctionCommand(new[]
            {
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
            });
        }

        public static FunctionCommand FunctionCall(string functionName, int numberOfParameters)
        {
            return null;
        }
    }
}
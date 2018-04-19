using System.Collections.Generic;

namespace VirtualMachine
{
    public static partial class Commands
    {
        public static Command Label(Label label) =>
            new Command($"label {label.Text}", new[] {label.Declaration});

        public static Command Goto(Label label)
        {
            return new Command(
                $"goto {label.Text}",
                new[]
                {
                    label.Address,
                    "0;JMP"
                });
        }

        public static Command IfGoto(Label label)
        {
            return new Command(
                $"if-goto {label.Text}",
                new[]
                {
                    "@SP",
                    "AM=M-1",
                    "D=M",
                    label.Address,
                    "D;JNE"
                });
        }

        public static Command FunctionCall(
            string name, int argc, LabelGenerator generator)
        {
            var label = generator.Generate("ret");
            return new Command(
                $"call {name} {argc}",
                new[]
                {
                    label.Address,
                    "D=A",
                    "@R13",
                    "M=D",

                    '@' + argc.ToString(),
                    "D=A",
                    "@R14",
                    "M=D",

                    '@' + name,
                    "D=A",
                    "@R15",
                    "M=D",

                    "@CALL",
                    "0;JMP",

                    label.Declaration
                });
        }

        public static Command DeclareFunction(string functionName, int argc)
        {
            var instructions = new List<string> {$"({functionName})"};
            if (argc == 0) return new Command($"function {functionName} 0", instructions);

            instructions.Add("@SP");
            instructions.Add("A=M");

            for (var i = argc; i > 0; i--)
            {
                instructions.Add("M=0");
                instructions.Add("AD=A+1");
            }

            instructions.Add("@SP");
            instructions.Add("M=D");

            return new Command($"function {functionName} {argc}", instructions);
        }

        public static Command FunctionRetrun()
        {
            return new Command(
                "return",
                new[]
                {
                    "@RETURN",
                    "0;JMP"
                });
        }
    }
}
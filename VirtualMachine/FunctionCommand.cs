using System.Collections.Generic;

namespace VirtualMachine
{
    public class FunctionCommand
    {
        public static Command Call(
            string calleeName, int numberOfArguments, LabelGenerator generator)
        {
            var label = generator.Generate();
            return new Command(new[]
            {
                label.Address,
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

                $"@{calleeName}",
                "0;JMP",

                label.Declaration
            });
        }

        public static Command DeclareFunction(string functionName, int numberOfParameters)
        {
            var instructions = new List<string> {$"({functionName})"};
            if (numberOfParameters == 0) return new Command(instructions);
            
            instructions.Add("@SP");
            instructions.Add("A=M");

            while (numberOfParameters > 0)
            {
                instructions.Add("M=0");
                instructions.Add("AD=A+1");
                numberOfParameters--;
            }
            
            instructions.Add("@SP");
            instructions.Add("M=D");
            
            return new Command(instructions);
        }

        public static Command Retrun()
        {
            return new Command(new[]
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
    }
}
using System.Collections.Generic;
using System.Linq;

namespace VirtualMachine
{
    public class FunctionCommand : Command
    {
        private FunctionCommand(IEnumerable<string> hackInstructions) : base(hackInstructions)
        {
        }

        public static Command Call(
            string calleeName, int numberOfArguments, LabelGenerator generator)
        {
            var label = generator.Generate();
            return new FunctionCommand(new[]
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
            var pushs = new List<string> {$"({functionName})"};
            if (numberOfParameters == 0) return new Command(pushs);
            
            pushs.Add("@SP");
            pushs.Add("A=M");

            while (numberOfParameters-- > 0)
            {
                pushs.Add("M=0");
                pushs.Add("AD=A+1");
            }
            
            pushs.Add("@SP");
            pushs.Add("M=D");
            
            return new Command(pushs);
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
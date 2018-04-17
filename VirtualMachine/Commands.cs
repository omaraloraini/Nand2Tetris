using System.Collections.Generic;

namespace VirtualMachine
{
    public static partial class Commands
    {
        public static Command Label(Label label) =>
            new Command(new[] {label.Declaration});
        
        public static Command Goto(Label label)
        {
            return new Command(new[]
            {
                label.Address,
                "0;JMP"
            });
        }
        
        public static Command IfGoto(Label label)
        {
            return new Command(new[]
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
            var label = generator.Generate();
            return new Command(new[]
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
            if (argc == 0) return new Command(instructions);
            
            instructions.Add("@SP");
            instructions.Add("A=M");

            while (argc > 0)
            {
                instructions.Add("M=0");
                instructions.Add("AD=A+1");
                argc--;
            }
            
            instructions.Add("@SP");
            instructions.Add("M=D");
            
            return new Command(instructions);
        }

        public static Command FunctionRetrun()
        {
            return new Command(new[]
            {
                "@RETURN",
                "0;JMP"
            });
        }
    }
}
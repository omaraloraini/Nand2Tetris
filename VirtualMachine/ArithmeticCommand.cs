using System.Collections.Generic;

namespace VirtualMachine
{
    public class ArithmeticCommand : Command
    {
        private ArithmeticCommand(IEnumerable<string> hackInstructions) : base(hackInstructions)
        {
        }

        public static ArithmeticCommand Add()
        {
            return new ArithmeticCommand(
                new[]
                {
                    "@SP",
                    "M=M-1",
                    "A=M",
                    "D=M",
                    "A=A-1",
                    "M=D+M"
                });
        }

        public static ArithmeticCommand Sub()
        {
            return new ArithmeticCommand(
                new[]
                {
                    "@SP",
                    "M=M-1",
                    "A=M",
                    "D=-M",
                    "A=A-1",
                    "M=D+M"
                });
        }

        public static ArithmeticCommand Neg()
        {
            return new ArithmeticCommand(new[]
            {
                "@SP",
                "A=M-1",
                "M=-M"
            });
        }
    }
}
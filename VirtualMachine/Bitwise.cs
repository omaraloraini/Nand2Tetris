namespace VirtualMachine
{
    public static partial class Commands
    {
        public class Bitwise
        {
            public static Command And()
            {
                return new Command(
                    "and",
                    new[]
                    {
                        "@SP",
                        "AM=M-1",
                        "D=M",
                        "A=A-1",
                        "M=D&M"
                    });
            }

            public static Command Or()
            {
                return new Command(
                    "or",
                    new[]
                    {
                        "@SP",
                        "AM=M-1",
                        "D=M",
                        "A=A-1",
                        "M=D|M"
                    });
            }

            public static Command Not()
            {
                return new Command(
                    "not",
                    new[]
                    {
                        "@SP",
                        "A=M-1",
                        "M=!M"
                    });
            }
        }
    }
}
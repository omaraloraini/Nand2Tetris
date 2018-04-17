namespace VirtualMachine
{
    public static partial class Commands
    {
        public static class Arithmetic
        {
            public static Command Add()
            {
                return new Command(
                    "add",
                    new[]
                    {
                        "@SP",
                        "AM=M-1",
                        "D=M",
                        "A=A-1",
                        "M=D+M"
                    });
            }

            public static Command Sub()
            {
                return new Command(
                    "sub",
                    new[]
                    {
                        "@SP",
                        "AM=M-1",
                        "D=-M",
                        "A=A-1",
                        "M=D+M"
                    });
            }

            public static Command Neg()
            {
                return new Command(
                    "neg",
                    new[]
                    {
                        "@SP",
                        "A=M-1",
                        "M=-M"
                    });
            }
        }
    }
}
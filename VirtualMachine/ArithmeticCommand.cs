namespace VirtualMachine
{
    public class ArithmeticCommand
    {
        public static Command Add()
        {
            return new Command(
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
            return new Command(new[]
            {
                "@SP",
                "A=M-1",
                "M=-M"
            });
        }
    }
}
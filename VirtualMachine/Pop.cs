namespace VirtualMachine
{
    public static partial class Commands
    {
        public static class Pop
        {
            public static Command Local(int index) =>
                PopToSegment($"pop local {index}", "@LCL", index);

            public static Command Argument(int index) =>
                PopToSegment($"pop argument {index}", "@ARG", index);

            public static Command This(int index) =>
                PopToSegment($"pop this {index}", "@THIS", index);

            public static Command That(int index) =>
                PopToSegment($"pop that {index}", "@THAT", index);

            public static Command Temp(int index) =>
                PopToLabel($"pop temp {index}", '@' + (5 + index).ToString());

            public static Command Static(string fileName, int index) =>
                PopToLabel($"pop static {index}", $"{fileName}.{index}");

            public static Command Pointer(int index) =>
                index == 0
                    ? PopToLabel("pop pointer 0", "@THIS")
                    : PopToLabel("pop pointer 1", "@THAT");

            private static Command PopToLabel(string name, string label)
            {
                label = label.StartsWith('@') ? label : '@' + label;
                return new Command(
                    name,
                    new[]
                    {
                        "@SP",
                        "AM=M-1",
                        "D=M",
                        label,
                        "M=D"
                    });
            }

            private static Command PopToSegment(string name, string baseAddress, int offset)
            {
                baseAddress = baseAddress.StartsWith('@') ? baseAddress : '@' + baseAddress;
                var offsetAddress = '@' + offset.ToString();

                return new Command(
                    name,
                    new[]
                    {
                        baseAddress,
                        "D=M",
                        offsetAddress,
                        "D=D+A",
                        "@SP",
                        "M=M-1",
                        "A=M+1",
                        "M=D",
                        "A=A-1",
                        "D=M",
                        "A=A+1",
                        "A=M",
                        "M=D"
                    });
            }
        }
    }
}
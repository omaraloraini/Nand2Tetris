namespace VirtualMachine
{
    public static partial class Commands
    {
        public static class Push
        {
            public static Command Constant(int constant)
            {
                return new Command(
                    $"push constant {constant}",
                    new[]
                    {
                        '@' + constant.ToString(),
                        "D=A",
                        "@SP",
                        "M=M+1",
                        "A=M-1",
                        "M=D"
                    });
            }

            public static Command Local(int index) =>
                PushFromSegment($"push local {index}", "@LCL", index);

            public static Command Argument(int index) =>
                PushFromSegment($"push argument {index}", "@ARG", index);

            public static Command This(int index) =>
                PushFromSegment($"push this {index}", "@THIS", index);

            public static Command That(int index) =>
                PushFromSegment($"push that {index}", "@THAT", index);

            public static Command Temp(int index) =>
                PushFromLabel($"push temp {index}", '@' + (5 + index).ToString());

            public static Command Static(string fileName, int index) =>
                PushFromLabel($"push static {index}", $"{fileName}.{index}");

            public static Command Pointer(int index) =>
                index == 0
                    ? PushFromLabel("push pointer 0", "@THIS")
                    : PushFromLabel("push pinter 1", "@THAT");

            private static Command PushFromLabel(string name, string label)
            {
                label = label.StartsWith('@') ? label : '@' + label;
                return new Command(
                    name,
                    new[]
                    {
                        label,
                        "D=M",
                        "@SP",
                        "M=M+1",
                        "A=M-1",
                        "M=D"
                    });
            }

            private static Command PushFromSegment(string name, string baseAddress, int offset)
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
                        "A=D+A",
                        "D=M",
                        "@SP",
                        "M=M+1",
                        "A=M-1",
                        "M=D",
                    });
            }
        }
    }
}
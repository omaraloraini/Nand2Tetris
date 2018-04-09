namespace VirtualMachine
{
    public static partial class Commands
    {
        public static class Pop
        {
            public static Command Local(int index) => PopToSegment("@LCL", index);
            public static Command Argument(int index) => PopToSegment("@ARG", index);
            public static Command This(int index) => PopToSegment("@THIS", index);
            public static Command That(int index) => PopToSegment("@THAT", index);
            public static Command Temp(int index) => PopToLabel('@' + (5 + index).ToString());
            public static Command Static(string fileName, int index) => 
                PopToLabel($"{fileName}.{index}");
        
            public static Command Pointer(int index) =>
                index == 0
                    ? PopToLabel("@THIS")
                    : PopToLabel("@THAT");
        
            private static Command PopToLabel(string label)
            {
                label = label.StartsWith('@') ? label : '@' + label;
                return new Command(new[]
                {
                    "@SP",
                    "AM=M-1",
                    "D=M",
                    label,
                    "M=D"
                });
            }

            private static Command PopToSegment(string baseAddress, int offset)
            {
                baseAddress = baseAddress.StartsWith('@') ? baseAddress : '@' + baseAddress;
                var offsetAddress = '@' + offset.ToString();
            
                return new Command(new[]
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